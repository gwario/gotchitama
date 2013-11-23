\ gt-misc.fs contains various helper words

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
: get-health-diff ( timediff-usec -- diff )
   0 ( timediff-usec 0 ) d>f ( |F timediff-usec )
   3e fswap ( |F 3 timediff-usec )
   [ 60 60 1000 1000 * * * ] literal 0 d>f ( |F 3 timediff-usec 1h-usec )
   f/       \ ( |F 3 timediff-hours )
   9e f+   \ ( |F 3 timediff-hours+9 )
   9e f/    \ ( |F 3 (timediff-hours+9)/9 )
   f**      \ ( |F 3^(timediff-hours+9)/9 )
   2.5e f-    \ ( |F (3^(timediff-hours+9)/9)+2.5 )
   fabs f>d ( health-diff e ) drop ( health-diff )
;


\ returns the happiness reduction for the passed time
\ 3^((x+9)/15)-1.1, where x is the passed time in hours
\ use http://www.snappymaria.com/canvas/FunctionPlotter.html for tweaking... pow(3,(x+9)/15)-1.1
: get-happiness-diff ( timediff-usec -- diff )
   0 ( timediff-usec 0 ) d>f ( |F timediff-usec )
   3e fswap ( |F 3 timediff-usec )
   [ 60 60 1000 1000 * * * ] literal 0 d>f ( |F 3 timediff-usec 1h-usec )
   f/       \ ( |F 3 timediff-hours )
   9e f+   \ ( |F 3 timediff-hours+9 )
   15e f/    \ ( |F 3 (timediff-hours+9)/15 )
   f**      \ ( |F 3^(timediff-hours+9)/15 )
   1.1e f-    \ ( |F (3^(timediff-hours+9)/15)-1.1 )
   fabs f>d ( health-diff e ) drop ( health-diff )
;

\ prints an overview of health and happiness reduction per hour
: #show-health-happiness-reduction-per-hour cr
   get-timestamp 1000 ms get-timestamp swap - 60 * 60 * ( 1-hour-usec )
   49 1 do
      dup i * get-health-diff i . ."  hour(s): -" . ." health and "
      dup i * get-happiness-diff ." -" . ." happiness" cr
   loop
;
