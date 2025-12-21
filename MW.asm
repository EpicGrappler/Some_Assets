; PARK0_9.ASM  -- 8086 .COM program for EMU8086
; org 100h
org 100h

start:
    ; set DS = CS for .COM program
    mov ax, cs
    mov ds, ax
    mov es, ax

main_loop:
    ; print prompt
    mov dx, prompt
    mov ah, 09h
    int 21h

    ; read buffered line (max 1 char)
    lea dx, inbuf
    mov ah, 0Ah
    int 21h

    ; get count
    mov al, [inbuf+1]    ; number of chars read
    cmp al, 0
    je main_loop         ; nothing read, repeat

    ; get the first character
    mov al, [inbuf+2]
    ; ignore CR if present (shouldn't be here with max=1)
    ; check for quit 'q' or 'Q'
    cmp al, 'q'
    je quit
    cmp al, 'Q'
    je quit

    ; check if digit '0'..'9'
    cmp al, '0'
    jb invalid_input
    cmp al, '9'
    ja invalid_input

    ; index = al - '0'  (0..9)
    sub al, '0'
    movzx di, al        ; zero-extend into DI (DI = index 0..9)

    ; address of spots array in DS
    lea bx, [spots]

    ; load spots[di]
    mov dl, [bx + di]
    cmp dl, 0
    je do_reserve

    ; currently reserved -> free it
do_free:
    mov byte [bx + di], 0
    ; print "Spot " then digit then " is now FREED."
    mov dx, msg_spot
    mov ah, 09h
    int 21h
    ; print digit
    mov al, [inbuf+2]    ; original char (ascii '0'..'9')
    mov dl, al
    mov ah, 02h
    int 21h
    mov dx, msg_freed
    mov ah, 09h
    int 21h
    jmp main_loop

do_reserve:
    mov byte [bx + di], 1
    ; print "Spot " then digit then " is now RESERVED."
    mov dx, msg_spot
    mov ah, 09h
    int 21h
    ; print digit
    mov al, [inbuf+2]
    mov dl, al
    mov ah, 02h
    int 21h
    mov dx, msg_reserved
    mov ah, 09h
    int 21h
    jmp main_loop

invalid_input:
    mov dx, invalid_msg
    mov ah, 09h
    int 21h
    jmp main_loop

quit:
    mov ah, 4Ch
    xor al, al
    int 21h

; -----------------------
; Data (strings must end with '$' for INT 21h AH=09h)
prompt       db 'Choose spot 0-9 to toggle, Q to quit: $'
msg_spot     db ' - Spot ', 0     ; we'll print the digit after
msg_reserved db ' is now RESERVED.$'
msg_freed    db ' is now FREED.$'
invalid_msg  db 'Invalid input. Enter 0..9 or Q to quit.$'

; DOS buffered input: first byte = max chars, second = actual count, then buffer bytes
inbuf db 1, 0, 0

; 10 parking spots: index 0..9
spots db 10 dup(0)

; end
