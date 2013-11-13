require mini-oof.fs

\ create word list
wordlist constant gotchitama

\ put our word list on top of the search order
get-order gotchitama swap 1+ set-order

\ Set current word list to gotchitama
gotchitama set-current

\ Write our words

object class
   \ health points: 0-100
   cell var health
   \ maths skills: 0-100
   cell var maths
   cell var happiness
end-class gotchitama


\ takes some "genes" parameters like cleverness from stack and creates an according gotchitama
\ : create-gotchi

\ create a gotchitama with random "genes"
\ : create-random-gotchi
   



