\ gt-iconst.fs contains some helper words to create an interactive constants
create constant-len
create constant$

\ START probably dirty code
\ creates a constant whose name is provided by the user via input stream
\ the constant's name is stored at constant$ with a length of constant-len

: interactive-constant ( x -- )
   s" constant " pad ( addr u addr-pad ) swap move \ moves the string "constant " to the scratchpad
   ." Enter your gotchitama's name: " pad 9 + ( addr-after-"constant " ) 80 accept ( len of <user-input> ) cr
   dup constant-len ! \ remember the length of the user-input
   constant-len @ allot \ create a buffer for the constant
   pad 9 + constant$ constant-len @ move \ move the user input to the buffer
   pad swap 9 + ( addr-pad len"constant <user-input>" ) evaluate
;

: interactive-gotchitama ( -- o )
   constant$ constant-len @ evaluate
;
\ END probable dirty code
