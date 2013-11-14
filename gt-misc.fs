\ gt-misc.fs contains various helper words

\ puts the current time in micro seconds onto the data stack
: get-timestamp ( -- timestamp )
   utime drop
;
