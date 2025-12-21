.model small                          ; Define memory model as small (code and data each fit in 64KB)
.stack 100h                           ; Allocate 256 bytes for the stack
.data                                 ; Start of data segment

; 13 => Carriage Return (CR) it return the cursor to the start of the line
; 10 => Line Feed (LF) it go down 1 line
menu    db 13,10, '1) Add two numbers',13,10    ; Menu string with newline at start, option 1
        db '2) Find max',13,10                   ; Menu option 2
        db '3) Print numbers 1 to 10',13,10      ; Menu option 3
        db '4) Exit',13,10                       ; Menu option 4
        db 'Choose: $'                           ; Prompt for user input, '$' marks end of string

msg_num1 db 13,10, 'Enter first number: $'      ; Prompt message for first number input
msg_num2 db 13,10, 'Enter second number: $'     ; Prompt message for second number input
msg_sum  db 13,10, 'Sum = $'                    ; Message to display before showing sum
msg_max  db 13,10, 'Max = $'                    ; Message to display before showing max
msg_exit db 13,10, 'Exiting...$'                ; Exit message

newline db 13,10,'$'                            ; Newline string (CR + LF + terminator)

; buffers
num1 db ?                             ; Variable to store first number (1 byte, uninitialized)
num2 db ?                             ; Variable to store second number (1 byte, uninitialized)
result db ?                           ; Variable to store result (1 byte, uninitialized)

.code                                 ; Start of code segment
main proc                             ; Main procedure declaration
    mov ax, @data                     ; Load address of data segment into AX
    mov ds, ax                        ; Initialize DS register with data segment address

start_menu:                           ; Label for menu loop start
    ; Print menu
    mov dx, offset menu               ; Load address of menu string into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print menu

    ; Read choice (single char)
    mov ah, 1                         ; DOS function 1: read character with echo
    int 21h                           ; Call DOS interrupt, character returned in AL
    sub al, '0'                       ; Convert ASCII character to numeric value (0..9)
    mov bl, al                        ; Store the choice in BL register

    ; Check options
    cmp bl, 1                         ; Compare choice with 1
    je add_numbers                    ; Jump to add_numbers if choice equals 1
    cmp bl, 2                         ; Compare choice with 2
    je find_max                       ; Jump to find_max if choice equals 2
    cmp bl, 3                         ; Compare choice with 3
    je print_1_to_10                  ; Jump to print_1_to_10 if choice equals 3
    cmp bl, 4                         ; Compare choice with 4
    je exit_program                   ; Jump to exit_program if choice equals 4

    ; invalid ? show menu again
    jmp start_menu                    ; Invalid choice, jump back to show menu again

; ----------------------------
; OPTION 1: Add Two Numbers           ;asci '0' = 95, '1' = 96
; ----------------------------
add_numbers:                          ; Label for addition function
    ; Enter first number
    mov dx, offset msg_num1           ; Load address of first number prompt into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print prompt

    mov ah, 1                         ; DOS function 1: read character with echo
    int 21h                           ; Call DOS interrupt, character returned in AL
    sub al, '0'                       ; Convert ASCII to numeric value
    mov num1, al                      ; Store first number in num1 variable

    ; Enter second number
    mov dx, offset msg_num2           ; Load address of second number prompt into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print prompt

    mov ah, 1                         ; DOS function 1: read character with echo
    int 21h                           ; Call DOS interrupt, character returned in AL
    sub al, '0'                       ; Convert ASCII to numeric value
    mov num2, al                      ; Store second number in num2 variable

    ; SUM = num1 + num2
    mov al, num1                      ; Load first number into AL
    add al, num2                      ; Add second number to AL, result in AL (0..18)
    ; convert to ASCII: handle possibly two digits (0..18)
    cmp al, 9                         ; Compare sum with 9
    jg sum_two_digits                 ; Jump if sum > 9 (needs two digits)

    ; single digit sum
    add al, '0'                       ; Convert single digit to ASCII
    mov result, al                    ; Store ASCII result
    mov dx, offset msg_sum            ; Load address of sum message into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print "Sum = "
    mov dl, result                    ; Load result character into DL
    mov ah, 2                         ; DOS function 2: print single character
    int 21h                           ; Call DOS interrupt to print result
    jmp add_done                      ; Jump to add_done to skip two-digit handling

sum_two_digits:                       ; Label for handling two-digit sum (10-18)
    ; convert to two ASCII digits
    ; AL contains 10..18
    mov ah, 0                         ; Clear AH for division (AX = AL, AH = 0)
    mov bl, 10                        ; Load divisor 10 into BL
    div bl                            ; Divide AX by BL: AL = quotient (tens), AH = remainder (ones)
    ; after div: AL = quotient, AH = remainder
    ; quotient in AL (1), remainder in AH
    add al, '0'                       ; Convert tens digit to ASCII
    mov bh, al                        ; Move tens digit to DL for printing
    mov bl, ah                        ; save the remainder value from overwrite
    mov dx, offset msg_sum            ; Load address of sum message into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print "Sum = "
    mov dl, bh  
    mov ah, 2                         ; DOS function 2: print single character
    int 21h                           ; Call DOS interrupt to print tens digit
    mov dl, bl                        ; Move remainder (ones digit) to DL
    add dl, '0'                       ; Convert ones digit to ASCII
    mov ah, 2                         ; DOS function 2: print single character
    int 21h                           ; Call DOS interrupt to print ones digit

add_done:                             ; Label after addition is complete
    ; newline
    mov dx, offset newline            ; Load address of newline string into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print newline

    jmp start_menu                    ; Jump back to main menu


; ----------------------------
; OPTION 2: Find Max
; ----------------------------
find_max:                             ; Label for find maximum function
    ; Enter first number
    mov dx, offset msg_num1           ; Load address of first number prompt into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print prompt

    mov ah, 1                         ; DOS function 1: read character with echo
    int 21h                           ; Call DOS interrupt, character returned in AL
    sub al, '0'                       ; Convert ASCII to numeric value
    mov num1, al                      ; Store first number in num1 variable

    ; Enter second number
    mov dx, offset msg_num2           ; Load address of second number prompt into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print prompt

    mov ah, 1                         ; DOS function 1: read character with echo
    int 21h                           ; Call DOS interrupt, character returned in AL
    sub al, '0'                       ; Convert ASCII to numeric value
    mov num2, al                      ; Store second number in num2 variable

    ; Compare numbers
    mov al, num1                      ; Load first number into AL
    mov bl, num2                      ; Load second number into BL
    cmp al, bl                        ; Compare AL with BL (num1 with num2)
    jge al_is_max                     ; Jump if AL >= BL (num1 is max or equal)
    mov al, bl                        ; Otherwise, move num2 (BL) to AL as max

al_is_max:                            ; Label: AL now contains the maximum value
    add al, '0'                       ; Convert max value to ASCII
    mov result, al                    ; Store ASCII result

    mov dx, offset msg_max            ; Load address of max message into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print "Max = "

    mov dl, result                    ; Load result character into DL
    mov ah, 2                         ; DOS function 2: print single character
    int 21h                           ; Call DOS interrupt to print max value

    ; newline
    mov dx, offset newline            ; Load address of newline string into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print newline

    jmp start_menu                    ; Jump back to main menu

; ----------------------------
; OPTION 3: Print 1 to 10  (FIXED)
; ----------------------------
print_1_to_10:                        ; Label for print 1-10 function
    mov cx, 10                        ; Set loop counter to 10 (for 10 numbers)
    mov bx, 1                         ; Initialize current number to 1
    ; newline
    mov dx, offset newline            ; Load address of newline string into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print newline

print_loop:                           ; Label for print loop start
    cmp bx, 10                        ; Compare current number with 10
    jne print_single_digit            ; Jump if not 10 (single digit)

    ; bx == 10: print '1' then '0'
    mov dl, '1'                       ; Load ASCII '1' into DL
    mov ah, 2                         ; DOS function 2: print single character
    int 21h                           ; Call DOS interrupt to print '1'
    mov dl, '0'                       ; Load ASCII '0' into DL
    mov ah, 2                         ; DOS function 2: print single character
    int 21h                           ; Call DOS interrupt to print '0'
    jmp after_print                   ; Jump to after_print section

print_single_digit:                   ; Label for printing single digit numbers
    ; print single digit: convert bx to ASCII
    mov ax, bx                        ; Copy current number (BX) to AX
    add al, '0'                       ; Convert low byte to ASCII
    mov dl, al                        ; Move ASCII character to DL
    mov ah, 2                         ; DOS function 2: print single character
    int 21h                           ; Call DOS interrupt to print digit

after_print:                          ; Label after number is printed
    ; print space
    mov dl, ' '                       ; Load ASCII space character into DL
    mov ah, 2                         ; DOS function 2: print single character
    int 21h                           ; Call DOS interrupt to print space

    inc bx                            ; Increment current number (BX = BX + 1)
    loop print_loop                   ; Decrement CX and loop if CX != 0

    ; newline
    mov dx, offset newline            ; Load address of newline string into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print newline

    jmp start_menu                    ; Jump back to main menu

; ----------------------------
; OPTION 4: Exit
; ----------------------------
exit_program:                         ; Label for exit function
    mov dx, offset msg_exit           ; Load address of exit message into DX
    mov ah, 9                         ; DOS function 9: print string
    int 21h                           ; Call DOS interrupt to print exit message

    mov ah, 4Ch                       ; DOS function 4Ch: terminate program
    int 21h                           ; Call DOS interrupt to exit to DOS

main endp                             ; End of main procedure
end main                              ; End of program, entry point is main