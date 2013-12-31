require mini-oof.fs
require gt-io.fs
require gt-misc.fs

true constant DEBUG

\ create word list
wordlist constant gotchitama

\ put our word list on top of the search order
get-order ( addr-n .. addr-n-i .. addr-0 n ) gotchitama ( addr-n .. addr-n-i .. addr-0 n addr-gt) swap 1+ set-order ( <0> )
\ Set current word list to gotchitama
gotchitama set-current

\ Write our words

object class
   cell var health                     \ health points: 0-100 default: 100
   cell var happiness                  \ happiness points: 0-100 default: 100
   2 cells var name                    \ pointer to the name of the gotchitama
   cell var len                        \ length of the name
   cell var time-of-last-interaction   \ the time of the last interaction with the gotchitama
   cell var birthtime                  \ the time of birth in micro seconds since epoch
   cell var cleverness                 \ cleverness points: 0-100 default: 10
   method init ( o -- )                      \ initializes vital values
   method feed ( food o -- )                 \ feeds the gotchitama TODO limit maximum food the gotchitama can get at a time; reduce happiness when feeding all the time
   method play ( o -- )                      \ play with the gotchitama increases happiness by 5 TODO: make it depending on the time since last playing, reduce health while playing
   method get-health ( o -- health )         \ puts the health onto the stack
   method get-happiness ( o -- happiness )   \ puts the happiness onto the stack
   method get-cleverness ( o -- cleverness ) \ puts the cleverness onto the stack
   method get-age ( o -- age )               \ puts the age in microseconds onto the stack
   method status ( o -- )                    \ prints status information
end-class gotchitama

\ This word will be compiled into every method
\ It should alter the variables considering the time since last action
: update ( o -- o ) >r
   r@ time-of-last-interaction @ get-timestamp  ( last-usec now-usec )
   dup r@ time-of-last-interaction ! ( last-usec now-usec )
   swap - ( timediff-usec )
   DEBUG if \ seconds like hours
      [ 60 60 * ] literal *
   endif
   r@ health @ over health-diff - at-least-zero r@ health !
   r@ happiness @ over happiness-diff - at-least-zero r@ happiness !
   r@ cleverness @ swap cleverness-diff - at-least-zero r@ cleverness !
   r>
;

\ types the name
: type-name ( o -- addr u ) >r
  r@ name r> len @ type
;
\ puts the name onto the stack
: get-name ( o -- addr u ) >r
  r@ name r> len @
;

\ checks whether or not gotchitama is still alive
: check-pulse ( o -- o ) >r
   r@ health @ 0 <=
   r@ happiness @ 0 <=
   or if
      cr color-red r@ type-name ."  is dead..." font-normal abort
   else
      r>
   endif 
;

\ Extends the mini-oof. Use :method instead of :noname if you want to
\ define a word which should auto-update the state of the gotchitama first
\ Uses trick similar to http://www.complang.tuwien.ac.at/forth/gforth/Docs-html/User_002ddefined-Defining-Words.html#User_002ddefined-Defining-Words
: :method :noname ['] update compile, ['] check-pulse compile, ;

\ initializes vital values with the defaults
:noname ( addr u o -- ) >r ( )
  100 r@ health ! \ set health
  100 r@ happiness ! \ set happiness
  10 r@ cleverness ! \ set cleverness
  dup
  r@ len ! \ set name length
  ( addr u ) chars r@ name swap move \ set name
  get-timestamp r@ birthtime ! \ set the time of birth
  get-timestamp r> time-of-last-interaction ! \ set timestamp
; gotchitama defines init


\ puts the health on top of the stack
:method ( o -- health )
   health @ ( health )
; gotchitama defines get-health

\ puts the happiness on top of the stack
:method ( o -- happiness )
   happiness @ ( happiness )
; gotchitama defines get-happiness

\ puts the cleverness on top of the stack
:method ( o -- cleverness )
   cleverness @ ( cleverness )
; gotchitama defines get-cleverness

\ puts the age in microseconds on top of the stack
:method ( o -- age )
  get-timestamp birthtime @ -
; gotchitama defines get-age


\ feeds the gotchitama
:method ( food o -- ) >r ( food ) 
   r@ health @ + ( food+health -- ) r> health ! ( )
; gotchitama defines feed

\ play with the gotchitama
:method ( o -- ) >r 
   r@ happiness @ 5 + ( happiness+5 -- ) r> happiness ! ( )
; gotchitama defines play


\ Print information on the status of the gotchitama
:method ( o -- ) >r cr
   ." Name:       " r@ type-name cr
   ." Health:     " r@ health @ . cr
   ." Happiness:  " r@ happiness @ . cr
   ." Cleverness: " r@ cleverness @ . cr
   ." Age:        " get-timestamp r> birthtime @ type-humanfriendly-timedifference
; gotchitama defines status


\ interactively asks the user for the gotchitama's name to write it into the given buffer address; accepts strings between 3 and 10 letters.
: ask-name ( c-addr -- c-addr u )
    ." Enter your gotchitama's name (3-10 characters): " dup 10 accept
    dup 10 > if s" gotchitama name too long" exception throw endif 
    dup 3 < if s" gotchitama name too short" exception throw endif ;

\ prints a log message; used when creating gotchitamas
: log-creation ( o -- )
    s" Created a new gotchitama named " pad place get-name pad +place pad count log ;

\ creates a new gotchitama, interactively asks for it's name
: create-gotchi ( -- )
    pad ask-name ( c-addr u )
    gotchitama new ( c-addr u o ) dup >R -rot ( o c-addr u | o ) ['] constant ( o c-addr u xt-constant | o ) execute-parsing ( | o ) \ o c-addr u xt-constant execute-parsing == 0 constant <user-input>
    latest ( nt-<user-input> | o ) name>string ( c-addr u | o )
    r@ ( c-addr u o | o ) init \ call init on o
    R>  log-creation
;




\ create-gotchi
\ print-avatar

\ just for debugging....
cr cr 
gotchitama new constant anton
s" anton" anton init
anton log-creation
print-avatar
