 create v4 104 c, 97 c, 108 c, 108 c, 111 c,

s" bla" pad swap move
\ pad 2 cells dump
\ pad .
\ here .

 : <constant ( addr u ) name-too-short? header, reveal docon: cfa, , ;
