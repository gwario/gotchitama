require mini-oof.fs

\ create word list
wordlist constant gotchitama

\ putt our word list on top of the search order
get-order mywords swap 1+ set-order

\ add word to word list
\ get-current gotchitama set-current ( wid )
\ create someword
\ ( wid ) set-current
