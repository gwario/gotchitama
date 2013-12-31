\ gt-misc.fs contains various helper words
include random.fs
\ initialize seed
utime drop here xor utime drop lshift seed !

\ puts the current time in micro seconds onto the data stack
: get-timestamp ( -- timestamp )
   utime drop
;

: millis  ( microseconds -- rest-usec milliseconds ) 1000 /mod ; 
: seconds ( microseconds -- rest-usec seconds ) [ 1000 1000 * ] literal /mod ;
: minutes ( microseconds -- rest-usec minutes ) [ 60 1000 1000 * * ] literal /mod ;
: hours   ( microseconds -- rest-usec hours ) [ 60 60 1000 1000 * * * ] literal /mod ;
: days    ( microseconds -- rest-usec days )  [ 24 60 60 1000 1000 * * * * ] literal /mod ;
: weeks   ( microseconds -- rest-usec weeks )  [ 7 24 60 60 1000 1000 * * * * * ] literal /mod ;
\ the following are kinda approximations
: months  ( microseconds -- rest-usec months ) [ 30 24 60 60 1000 1000 * * * * * ] literal /mod ;
: years   ( microseconds -- rest-usec years ) [ 365 24 60 60 1000 1000 * * * * * ] literal /mod ;

: ** ( n m -- n^m )
  1 swap  0 ?do over * loop  nip ;
: type-humanfriendly-timedifference ( time1 time2 -- )
   - ( time1 - time2 )
   years   dup 0> if . ." years, "  else drop endif ( rest-usec )
   months  dup 0> if . ." months, " else drop endif ( rest-usec )
   days    dup 0> if . ." days, "   else drop endif ( rest-usec )
   hours   dup 0> if . ." hours, "  else drop endif ( rest-usec )
   minutes           . ." minutes and " seconds . ." seconds." ( rest-usec )
   drop
;

\ returns the health reduction for the passed time
\ 3^((x+9)/9)-2.5, where x is the passed time in hours
\ use http://www.snappymaria.com/canvas/FunctionPlotter.html for tweaking... pow(3,(x+10)/9)+2.5
: health-diff ( timediff-usec -- diff )
   0 ( timediff-usec 0 ) d>f ( |F timediff-usec )
   3e fswap ( |F 3 timediff-usec )
   [ 60 60 1000 1000 * * * ] literal 0 d>f ( |F 3 timediff-usec 1h-usec )
   f/       \ ( |F 3 timediff-hours )
   9e f+    \ ( |F 3 timediff-hours+9 )
   9e f/    \ ( |F 3 (timediff-hours+9)/9 )
   f**      \ ( |F 3^(timediff-hours+9)/9 )
   2.5e f-  \ ( |F (3^(timediff-hours+9)/9)+2.5 )
   fabs f>d ( health-diff e ) drop ( health-diff )
;


\ returns the happiness reduction for the passed time
\ 3^((x+9)/15)-1.1, where x is the passed time in hours
\ use http://www.snappymaria.com/canvas/FunctionPlotter.html for tweaking... pow(3,(x+9)/15)-1.1
: happiness-diff ( timediff-usec -- diff )
   0 ( timediff-usec 0 ) d>f ( |F timediff-usec )
   3e fswap ( |F 3 timediff-usec )
   [ 60 60 1000 1000 * * * ] literal 0 d>f ( |F 3 timediff-usec 1h-usec )
   f/       \ ( |F 3 timediff-hours )
   9e f+    \ ( |F 3 timediff-hours+9 )
   15e f/   \ ( |F 3 (timediff-hours+9)/15 )
   f**      \ ( |F 3^(timediff-hours+9)/15 )
   1.1e f-  \ ( |F (3^(timediff-hours+9)/15)-1.1 )
   fabs f>d ( health-diff e ) drop ( health-diff )
;

\ returns the cleverness reduction for the passed time
\ (x/4)+0.5, where x is the passed time in hours
\ use http://www.snappymaria.com/canvas/FunctionPlotter.html for tweaking... x/4+0.5
: cleverness-diff ( timediff-usec -- diff )
   0 ( timediff-usec 0 ) d>f ( |F timediff-usec )
   [ 60 60 1000 1000 * * * ] literal 0 d>f ( |F timediff-usec 1h-usec )
   f/       \ ( |F timediff-hours )
   4e f/    \ ( |F timediff-hours/4 )
   0.5e f+  \ ( |F (timediff-hours/4)+0.5 )
   fabs f>d ( cleverness-diff e ) drop ( cleverness-diff )
;

\ determine the difference from a number taking cleverness value into account
: solution-diff ( solution cleverness -- solution-diff)
   101 swap - dup cr ." deviation of the proper solution [ 0 , " . ." ]%" ( solution 101-cleverness ) random ( solution rnd(0..101-cleverness-1 )
   0 d>f 0 d>f ( |F rnd(0..101-cleverness-1 solution ) 100e f/ ( |F rnd(0..101-cleverness-1 solution-1% )
   f* ( |F solution-rnd(0..100-cleverness-1% )
   1 random if
      fnegate
   endif
   fround
   f>d ( deviation ) drop
   cr ." absolute deviation is " dup .
   ( +/-deviation )
;

\ takes an integer from the stack. if the number is positive, it will remain unchanged and if it is negative it is replaced by 0.
: at-least-zero ( n -- n/0 )
   dup 0< if
      drop 0
   endif
;

\ takes an integer from the stack. if the number is less than 100, it will remain unchanged and if it is greater than 100 it is replaced by 100.
: 100-at-maximum ( n -- n/100 )
   dup 100 > if
      drop 100
   endif
;

\ prints an overview of health and happiness reduction per hour
: #show-health-happiness-reduction-per-hour cr
   get-timestamp 1000 ms get-timestamp swap - [ 60 60 * ] literal * ( 1-hour-usec )
   49 1 do
      dup i * health-diff i . ."  hour(s): -" . ." health and "
      dup i * happiness-diff ." -" . ." happiness" cr
   loop
;

\ prints avatar as ascii art
: print-avatar
   ."                                              `                " cr
   ."      +@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@`" cr
   ."    ,@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@." cr
   ."   .@@@@,..................,.                            .``   " cr
   ."   @@@#                                                        " cr
   ."  ,@@@`                                                        " cr
   ."  ;@@@                                                   `@@'  " cr
   ."  #@@#         ;@@@@@+             .+@@@@@#.             ;@@@  " cr
   ."  #@@#       #@@@@@@@@@+          @@@@@@@@@@#            `@@@  " cr
   ."  @@@'      @@@@@,.,#@@@@        ;@@@+: .'@@@@           `@@@  " cr
   ."  @@@'      @@@`      +@:         ,,       ;+`           ;@@@  " cr
   ."  @@@'                                                   +@@@  " cr
   ."  @@@;                                                   +@@@  " cr
   ."  @@@:                                                   @@@,  " cr
   ."  @@@,                                                   @@@.  " cr
   ."  @@@,                                                  .@@@`  " cr
   ."  @@@,                           `+@@@'                 '@@@   " cr
   ."  @@@,             ;@@,       ;@@@@@@@,                 @@@'   " cr
   ."  @@@;             #@@@  .;@@@@@@@@+                   '@@@    " cr
   ."  @@@+              @@@@@@@@@@@#'                     `@@@:    " cr
   ."  '@@@              ,@@@@@@@:                         @@@@     " cr
   ."   @@@:                ``                            '@@@.     " cr
   ."   #@@@                                             ;@@@;      " cr
   ."   `@@@:                                           +@@@'       " cr
   ."    +@@@;                                       .#@@@@+        " cr
   ."     #@@@@#:                                  :@@@@@@.         " cr
   ."      ,@@@@@@@@@:`                     ``.:#@@@@@@+`           " cr
   ."         '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@,              " cr
   ."            `,'#@@@@@@@@@@@@@@@@@@@@@@@@@@@#+,                 " cr
   ."                   `.'''''''':```                              " cr
;

