require mini-oof.fs

\ create word list
wordlist constant gotchitama

\ put our word list on top of the search order
get-order gotchitama swap 1+ set-order

\ Set current word list to gotchitama
gotchitama set-current

\ Write our words
