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
   \ cell var maths                       \ maths skills: 0-100
   \ cell var happiness
   method init ( o -- )                \ initializes vital values
   method feed ( food o -- )           \ feeds the gotchitama
   method get-health ( o -- health )   \ puts the health onto the stack
end-class gotchitama

\ initializes vital values with the defaults
:noname ( o -- ) >r ( -- )
   100 r> health !
; gotchitama defines init

\ feeds the gotchitama
:noname ( food o -- ) >r ( food -- ) 
   r@ health @ + ( food+health -- ) r> health ! ( -- )
; gotchitama defines feed

\ puts the health on top of the stack
:noname ( o -- health )
   health @ ( health )
; gotchitama defines get-health


\ takes some "genes" parameters like cleverness from stack and creates an according gotchitama
\ : create-gotchi ( addr u -- addr ) ( name -- gotchitama )

\ create a gotchitama with random "genes"
\ : create-random-gotchi \ mga: what are these genes?
   

\ just for debugging....

cr cr 
gotchitama new constant anton
anton init
s" created a new gotchitama named anton" log

\ Creates a constant of a name stored in addr of length u
\ : <constant ( addr u -- ) name-too-short? header, reveal docon: cfa, , ;

cr cr
." Enter your gotchitama's name: " 
gotchitama new interactive-constant

\ create constant-len 2 cells allot
\ create constant$


\ : get-name pad pad 80 accept 
\     dup constant-len ! \ remember the length of the user-input
\     constant-len @ cells allot \ create a buffer for the constant
\     pad constant$ constant-len @ move \ move the user input to the buffer
\    cr ;

\ gotchitama new get-name \ <constant 
 s" created a new gotchitama named " pad place constant$ constant-len @ pad +place pad count log

\ interactive-gotchitama init
\ s" created a new gotchitama named " pad place constant$ constant-len @ pad +place pad count log

\ prints information on the given gotchitama's vital values
: status ( o -- ) cr
   ." Health: " get-health . cr
;

2500 ms page
