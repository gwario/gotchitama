require mini-oof.fs
require gt-io.fs
require gt-iconst.fs
require gt-misc.fs

\ create word list
wordlist constant gotchitama

\ put our word list on top of the search order
get-order ( addr-n .. addr-n-i .. addr-0 n ) gotchitama ( addr-n .. addr-n-i .. addr-0 n addr-gt) swap 1+ set-order ( <0> )

\ Set current word list to gotchitama
gotchitama set-current

\ Write our words

object class
   cell var health                     \ health points: 0-100 default: 100
   cell var name  
   cell var len
   cell var time
   \ cell var maths                       \ maths skills: 0-100
   \ cell var happiness
   method init ( o -- )                \ initializes vital values
   method feed ( food o -- )           \ feeds the gotchitama
   method get-health ( o -- health )   \ puts the health onto the stack
   method status ( o -- )
end-class gotchitama

\ This word will be compiled into every method
\ It should alter the variables considering the time since last action
: update ( o -- o ) >r
   r@ health @ 5 - r@ health ! r> ; \ TODO: make change dependent on time

\ Extends the mini-oof. Use :method instead of :noname if you want to
\ define a word which should auto-update the state of the gotchitama first
\ Uses trick similar to http://www.complang.tuwien.ac.at/forth/gforth/Docs-html/User_002ddefined-Defining-Words.html#User_002ddefined-Defining-Words
: :method :noname ['] update compile, ;

\ initializes vital values with the defaults
:noname ( addr u o -- ) >r ( -- )
  100 r@ health ! \ set health
  r@ len ! \ set name length
  get-timestamp r@ time ! \ set timestamp
  r> name ! \ set name
; gotchitama defines init

\ puts the health on top of the stack
:method ( o -- health )
   health @ ( health )
; gotchitama defines get-health

\ feeds the gotchitama
:method ( food o -- ) >r ( food -- ) 
   r@ health @ + ( food+health -- ) r> health ! ( -- )
; gotchitama defines feed

\ Print information on the status of the gotchitama
:method ( o -- ) cr ." Health:  " health @ . cr ; gotchitama defines status

\ just for debugging....

\ cr cr 
\ gotchitama new constant anton
\  s" Anton" anton init
\ s" created a new gotchitama named anton" log

: ask-name ( c-addr -- c-addr u )
    ." Enter your gotchitama's name (5-10 characters): " dup 10 accept
    dup 10 > if s" gotchitama name too long" exception throw endif 
    dup 5 < if s" gotchitama name too short" exception throw endif ;

: log-creation ( o -- )
    >R s" Created a new gotchitama named " pad place R@ name @ R> len @ pad +place pad count log ;

: create-gotchi ( -- )
    pad ask-name gotchitama new dup >R -rot  ['] constant execute-parsing ( )
    latest name>string r@ init
    R> log-creation ;


\ 2500 ms page

create-gotchi
