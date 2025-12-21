.model small
.stack 100h

.data         

    ; ===== Welcome & Menu Messages =====
    intro_msg db "Welcome to Smart Parking System! Manage your parking spots efficiently.",13,10,"Press any key to continue...",13,10,"$"  ; Welcome message
    menu_prompt db "Please select an option:", 13, 10, "$"   ; Main menu header
    option1 db "1. RESERVE SPOT - Reserve an available parking spot.", 13, 10, "$"  ; Menu option 1
    option2 db "2. FREE SPOT - Free a previously reserved spot.", 13, 10, "$"      ; Menu option 2
    option3 db "3. Reset All Spots - Set all spots to empty.", 13, 10, "$"         ; Menu option 3
    option4 db "4. Show Parking Status - View status of all parking spots.", 13, 10, "$"  ; Menu option 4
    option5 db "5. Exit - Close the program.", 13, 10, "$"                          ; Menu option 5

    ; ===== Error / Info Messages =====
    invalid_msg db 13, 10,"!!! Invalid option selected. Please try again. Press any key to continue. !!! ", 13, 10, "$"  ; Invalid input message
    msg_a db 13, 10, "Option A initial task completed.", 13, 10, "$"  ; Info message for Option A
    msg_b db 13, 10, "Option B initial task completed.", 13, 10, "$"  ; Info message for Option B
    msg_c db 13, 10, "Option C initial task completed.", 13, 10, "$"  ; Info message for Option C
    hello_world_msg db 13, 10, "hello world!", 13, 10, "$"            ; Test message

    ; ===== Final Action Menu =====
    final_action_prompt db 13,10,"--- Action Menu ---",13,10 , "1. Return to Main Menu", 13, 10, "2. Exit Program", 13, 10, "$"  ; Menu after task completion

    ; ===== Reservation / Freeing Messages =====
    ReservedSucces_msg db 13, 10, "This spot has been reserved.", 13, 10, "$"   ; Reservation success
    FreedSucces_msg    db 13, 10, "This spot has been Freed.", 13, 10, "$"      ; Freeing success
    ReservedError_msg  db 13, 10, "THIS SPOT IS ALREADY RESERVED.", 13, 10, "$" ; Error: spot reserved
    FreedError_msg     db 13, 10, "THIS SPOT IS ALREADY FREED.", 13,10, "$"     ; Error: spot free

    ; ===== Parking Spot Data =====
    n_spots        equ 10               ; Total number of spots
    parking_spots  db n_spots dup(0)    ; 0 = free, 1 = reserved
    inbuf db 1, 0, 0                    ; Input buffer

    empty_msg      db "Empty", "$"      ; Display empty
    reserved_msg   db "Reserved", "$"   ; Display reserved
    colon_space    db ": ", "$"         ; Colon separator
    spot_prefix    db "Spot ", "$"      ; Spot label
    newline        db 13,10,"$"         ; Newline

    ; ===== Spot Priority =====
    ; 0 = Normal, 1 = VIP, 2 = Disabled
    spot_priority  db 2,2,1,1,0,0,0,0,0,0
    prio_normal_msg   db " [Normal]", "$"
    prio_vip_msg      db " [VIP]", "$"
    prio_disabled_msg db " [Disabled]", "$"

    ; ===== Price Messages =====
    price_normal_msg   db 13,10,"Price: 10$",13,10,"$"
    price_vip_msg      db 13,10,"Price: 20$",13,10,"$"
    price_disabled_msg db 13,10,"Price: 5$",13,10,"$"

    ; ===== Reservation Control Variables =====
    selected_priority  db 0        ; Selected type 0/1/2
    found_flag         db 0        ; 0 = none found, 1 = at least one

    choose_type_prompt db 13,10,"Select type:",13,10,"1 - Normal",13,10,"2 - VIP",13,10,"3 - Disabled",13,10,"$" ; Prompt type
    no_spots_msg       db 13,10,"No empty spots for this type.",13,10,"$"                                    ; No spot available
    choose_prompt db 13,10,"Choose one of the available spots: $"                                             ; Prompt for spot selection

    ; ===== Reset Messages =====
    reset_confirm_msg db 13,10,"Reset all spots to EMPTY?",13,10,"1 - YES",13,10,"0 - NO",13,10,"$"
    reset_done_msg    db 13,10,"All spots have been reset to EMPTY.",13,10,"$"
    reset_cancel_msg  db 13,10,"Reset cancelled.",13,10,"$"

    ; ===== Payment Messages =====
    confirm_payment_msg db 13,10,"Confirm payment?",13,10,"1 - YES",13,10 ,"0 - NO: $"
    payment_success_msg db 13,10,"DONE,Payment confirmed!$",13,10,"$"
    payment_cancel_msg  db 13,10,"Payment cancelled. The spot is now available.",13,10,"$"

    ; ===== Session Analytics =====
    analytics_header db 13,10,"Session Analytics:",13,10,"$"
    analytics_type db "Type       Reserved  Free  Revenue",13,10,"$"
    analytics_normal db "Normal     ", "$"
    analytics_vip    db "VIP        ", "$"
    analytics_disabled db "Disabled   ", "$"
    analytics_total  db "Total      ", "$"

    reserved_normal db 0
    reserved_vip db 0
    reserved_disabled db 0                 

    ; ===== Parking Ticket Layout =====
    ticket_header_top    db 13,10,"====================================",13,10,"$"
    ticket_header_title  db "--- Parking Reservation Ticket ---",13,10,"$"
    ticket_spot_num      db "Spot Number: $",13,10,"$"
    ticket_spot_type     db "Spot Type: $",13,10,"$"
    ticket_spot_price    db "Price: $",13,10,"$"
    ticket_header_bottom db "====================================",13,10,"$"

    total_cars_count db 0
    spot_usage       db n_spots dup(0)

    analytics_title  db 13,10,"--- Session Analytics ---",13,10,"$"
    total_cars_msg   db "Total Cars Today: $"
    most_used_msg    db 13,10,"Most Used Spot: $"  
    thank_you_msg db 13,10, "Thank you for using Smart Parking System!", 13,10, "$"

    max_usage db 0
    max_index dw 0       ; word = 16-bit

.code


;----------------------------------------------------
main proc
    call InitData              ; initialize data and show welcome message

menu_loop:
    call ShowMenu              ; display main menu and get user choice

    cmp al, '1'
    je option_a                ; reserve spot
    cmp al, '2'
    je option_b                ; free spot
    cmp al, '3'
    je option_c                ; reset all spots
    cmp al, '4'
    je show_status_option      ; show parking status
    cmp al, '5'
    je program_exit            ; exit program
    cmp al, '6'
    je show_analytics          ; show session analytics

invalid_input:                         
    mov dx, offset invalid_msg ; show invalid input message
    mov ah, 9
    int 21h
    mov ah, 0
    int 16h                    ; wait for key press
    jmp menu_loop              ; return to menu

option_a:
    call ReserveFlow           ; handle reservation process
    jmp menu_loop

option_b:
    call FreeFlow              ; handle free spot process
    jmp menu_loop

option_c:
    call ResetAllSpots         ; reset all parking spots
    jmp menu_loop

show_status_option:
    call ShowStatus            ; display all parking spots
    call PromptFinalAction     ; ask user to continue or exit
    mov ah, 1
    int 21h
    sub al, '0'
    cmp al, 1
    je menu_loop               ; return to menu if selected

show_analytics:
    call ShowSessionAnalytics  ; display session analytics
    jmp menu_loop

program_exit:
    call ShowSessionAnalytics  ; show final analytics
    mov dx, offset thank_you_msg ; display thank you message
    mov ah, 09h
    int 21h
    mov ax, 4c00h              ; terminate program
    int 21h

main endp


;----------------------------------------------------
; InitData proc
; Initialize data segment and display welcome message
InitData proc
    mov ax, @data       ; load data segment address into AX
    mov ds, ax           ; set DS register to point to data segment

    mov dx, offset intro_msg  ; load offset of welcome message
    mov ah, 9                 ; DOS function 9: display string
    int 21h

    mov ah, 0        ; DOS function 0: wait for key press
    int 16h

    ret
InitData endp
;----------------------------------------------------
ShowMenu proc
    mov ah, 0
    mov al, 3
    int 10h              ; set text mode (clear screen)

    mov dx, offset menu_prompt
    mov ah, 9
    int 21h              ; display menu title

    mov dx, offset option1
    mov ah, 9
    int 21h              ; show option 1

    mov dx, offset option2
    mov ah, 9
    int 21h              ; show option 2

    mov dx, offset option3
    mov ah, 9
    int 21h              ; show option 3

    mov dx, offset option4
    mov ah, 9
    int 21h              ; show option 4

    mov dx, offset option5
    mov ah, 9
    int 21h              ; show option 5

    mov ah, 0
    int 16h              ; wait for key press (choice in AL)
    ret
ShowMenu endp


;----------------------------------------------------
functionA proc
    ; do nothing, message removed to avoid duplication
    ret
functionA endp


functionB proc
    mov dx, offset FreedSucces_msg
    mov ah, 09h
    int 21h
functionB endp

functionC proc
    mov dx, offset hello_world_msg
    mov ah, 9
    int 21h
    ret
functionC endp        

;----------------------------------------------------
PromptFinalAction proc
    mov dx, offset final_action_prompt
    mov ah, 9
    int 21h              ; display final action menu

    mov ah, 0
    int 16h              ; wait for user key input

    mov dl, al
    mov ah, 2
    int 21h              ; echo pressed key

    cmp al, '2'
    je program_exit      ; exit program if user selects option 2
    ret                  ; return to caller (main menu)
PromptFinalAction endp


;----------------------------------------------------

; ReadOption proc
; Read a key from the keyboard and convert ASCII to number
ReadOption proc
    mov ah, 0       ; BIOS: wait for keypress, read key into AL
    int 16h
    sub al, '0'     ; convert ASCII character '0'-'9' to numeric value 0-9
    ret
ReadOption endp

;----------------------------------------------------
ValidateSpot proc
    cmp al, '0'
    jb  VS_invalid        ; if input < '0', invalid

    cmp al, '9'
    ja  VS_invalid        ; if input > '9', invalid

    sub al, '0'           ; convert ASCII to number
    xor ah, ah            ; clear high byte
    mov di, ax            ; store spot index in DI

    clc                   ; clear carry flag (valid input)
    ret

VS_invalid:
    stc                   ; set carry flag (invalid input)
    ret
ValidateSpot endp

;----------------------------------------------------
PrintSpot proc
    ; print "Spot X: Status [Type]"

    mov dx, OFFSET spot_prefix
    mov ah, 09h
    int 21h              ; print "Spot "

    mov ax, di
    add al, '0'
    mov dl, al
    mov ah, 02h
    int 21h              ; print spot number

    mov dx, OFFSET colon_space
    mov ah, 09h
    int 21h              ; print ": "

    mov bx, OFFSET parking_spots
    mov al, [bx + di]
    cmp al, 0
    je PS_empty          ; check if spot is empty

    mov dx, OFFSET reserved_msg
    mov ah, 09h
    int 21h              ; print "Reserved"
    jmp PS_show_priority

PS_empty:
    mov dx, OFFSET empty_msg
    mov ah, 09h
    int 21h              ; print "Empty"

PS_show_priority:
    mov bx, OFFSET spot_priority
    mov al, [bx + di]    ; get spot type

    cmp al, 2
    je PS_disabled       ; disabled spot
    cmp al, 1
    je PS_vip            ; VIP spot

    mov dx, OFFSET prio_normal_msg
    mov ah, 09h
    int 21h              ; normal spot
    jmp PS_newline

PS_vip:
    mov dx, OFFSET prio_vip_msg
    mov ah, 09h
    int 21h              ; VIP spot
    jmp PS_newline

PS_disabled:
    mov dx, OFFSET prio_disabled_msg
    mov ah, 09h
    int 21h              ; disabled spot

PS_newline:
    mov dx, OFFSET newline
    mov ah, 09h
    int 21h              ; move to next line
    ret
PrintSpot endp


;----------------------------------------------------
; ShowStatus proc
; Display the status of all parking spots
ShowStatus proc
    mov cx, n_spots       ; set loop counter to total number of spots
    xor di, di            ; start index from 0

ShowStatus_loop:
    call PrintSpot        ; print current spot status
    inc di                ; move to next spot
    loop ShowStatus_loop  ; repeat for all spots
    ret
ShowStatus endp

;----------------------------------------------------
ReserveFlow proc
    mov dx, OFFSET choose_type_prompt
    mov ah, 09h
    int 21h                  ; ask user to select spot type

    mov ah, 1
    int 21h                  ; read user input

    cmp al, '1'
    je RF_type_normal        ; normal spot
    cmp al, '2'
    je RF_type_vip           ; VIP spot
    cmp al, '3'
    je RF_type_disabled      ; disabled spot
    jmp ReserveFlow_invalid  ; invalid input

RF_type_normal:
    mov selected_priority, 0 ; set normal type
    jmp RF_search

RF_type_vip:
    mov selected_priority, 1 ; set VIP type
    jmp RF_search

RF_type_disabled:
    mov selected_priority, 2 ; set disabled type

RF_search:
    mov cx, n_spots
    xor di, di
    mov found_flag, 0        ; no spot found yet

RF_loop:
    mov bx, OFFSET spot_priority
    mov al, [bx + di]
    cmp al, selected_priority
    jne RF_next              ; skip if type does not match

    mov bx, OFFSET parking_spots
    mov al, [bx + di]
    cmp al, 0
    jne RF_next              ; skip if spot is not empty

    mov found_flag, 1
    mov byte ptr [bx + di], 1 ; reserve the spot

    call functionA           ; extra action (optional)

    mov dx, OFFSET spot_prefix
    mov ah, 09h
    int 21h                  ; print "Spot "

    mov ax, di
    add al, '0'
    mov dl, al
    mov ah, 02h
    int 21h                  ; print spot number

    mov dx, OFFSET newline
    mov ah, 09h
    int 21h

    mov bx, OFFSET spot_priority
    mov al, [bx + di]
    cmp al, 2
    je RF_price_disabled     ; disabled price
    cmp al, 1
    je RF_price_vip          ; VIP price

    mov dx, OFFSET price_normal_msg
    jmp RF_price_print

RF_price_vip:
    mov dx, OFFSET price_vip_msg
    jmp RF_price_print

RF_price_disabled:
    mov dx, OFFSET price_disabled_msg

RF_price_print:
    mov ah, 09h
    int 21h                  ; display price

    call ShowTicketAndConfirm ; show ticket and confirm payment
    call PromptFinalAction    ; ask user to continue or exit
    ret

RF_next:
    inc di
    loop RF_loop             ; check next spot

    mov dx, OFFSET no_spots_msg
    mov ah, 09h
    int 21h                  ; no available spots message
    call PromptFinalAction
    ret

ReserveFlow_invalid:
    mov dx, OFFSET invalid_msg
    mov ah, 09h
    int 21h                  ; invalid input message
    ret
ReserveFlow endp


;----------------------------------------------------
FreeFlow proc
    call ShowStatus              ; display all parking spots

    mov dx, offset choose_prompt
    mov ah, 09h
    int 21h                      ; ask user to choose a spot

    mov ah, 1
    int 21h                      ; read user input

    call ValidateSpot
    jc  FreeFlow_invalid         ; invalid spot number

    mov bx, offset parking_spots
    mov al, [bx + di]
    cmp al, 1
    jne FreeFlow_already_Available ; spot is already free

    mov byte ptr [bx + di], 0    ; free the selected spot
    call PromptFinalAction       ; ask user to continue or exit
    ret

FreeFlow_already_Available:
    mov dx, offset FreedError_msg
    mov ah, 09h
    int 21h                      ; show already free message
    call PromptFinalAction
    ret

FreeFlow_invalid:
    mov dx, offset invalid_msg
    mov ah, 09h
    int 21h                      ; show invalid input message
    ret
FreeFlow endp


;----------------------------------------------------
ResetAllSpots proc
    ; Step 1: show reset confirmation message
    mov dx, offset reset_confirm_msg
    mov ah, 09h
    int 21h

    ; Step 2: read user input
    mov ah, 1
    int 21h

    ; Step 3: check user choice
    cmp al, '1'
    je RAS_do_reset          ; user confirmed reset
    cmp al, '0'
    je RAS_cancel            ; user cancelled reset

    mov dx, offset invalid_msg
    mov ah, 09h
    int 21h                  ; invalid input
    call PromptFinalAction
    ret

RAS_do_reset:
    mov cx, n_spots
    mov bx, OFFSET parking_spots
    xor di, di               ; start from first spot

RAS_loop:
    mov byte ptr [bx + di], 0 ; set spot to empty
    inc di
    loop RAS_loop

    mov dx, OFFSET reset_done_msg
    mov ah, 09h
    int 21h                  ; reset completed message

    call PromptFinalAction
    ret

RAS_cancel:
    mov dx, OFFSET reset_cancel_msg
    mov ah, 09h
    int 21h                  ; reset cancelled message
    call PromptFinalAction
    ret
ResetAllSpots endp



;----------------------------------------------------
ShowTicketAndConfirm proc
    push ax
    push bx
    push dx               ; save registers

    ; ===== Ticket Header =====
    mov dx, offset ticket_header_top
    mov ah, 09h
    int 21h               ; print top border

    mov dx, offset ticket_header_title
    mov ah, 09h
    int 21h               ; print ticket title

    mov dx, offset ticket_header_bottom
    mov ah, 09h
    int 21h               ; print bottom border
    ; =========================

    mov dx, offset newline
    mov ah, 09h
    int 21h               ; new line

    mov dx, offset ReservedSucces_msg
    mov ah, 09h
    int 21h               ; show reservation message

    mov dx, offset newline
    mov ah, 09h
    int 21h               ; new line

    mov dx, offset spot_prefix
    mov ah, 09h
    int 21h               ; print "Spot "

    mov ax, di
    add al, '0'
    mov dl, al
    mov ah, 02h
    int 21h               ; print spot number

    mov dx, offset colon_space
    mov ah, 09h
    int 21h               ; print ": "

    mov bx, offset spot_priority
    mov al, [bx+di]       ; get spot type

    cmp al, 1
    je ST_VIP             ; VIP spot
    cmp al, 2
    je ST_Disabled        ; disabled spot

    mov dx, offset prio_normal_msg
    jmp ST_ShowType

ST_VIP:
    mov dx, offset prio_vip_msg
    jmp ST_ShowType

ST_Disabled:
    mov dx, offset prio_disabled_msg

ST_ShowType:
    mov ah, 09h
    int 21h               ; display spot type

    mov bx, offset spot_priority
    mov al, [bx+di]       ; get spot type again

    cmp al, 1
    je ST_PriceVIP        ; VIP price
    cmp al, 2
    je ST_PriceDisabled   ; disabled price

    mov dx, offset price_normal_msg
    jmp ST_ShowPrice

ST_PriceVIP:
    mov dx, offset price_vip_msg
    jmp ST_ShowPrice

ST_PriceDisabled:
    mov dx, offset price_disabled_msg

ST_ShowPrice:
    mov ah, 09h
    int 21h               ; display price

    mov dx, offset confirm_payment_msg
    mov ah, 09h
    int 21h               ; ask for payment confirmation

    mov ah, 01h
    int 21h               ; read user input

    cmp al, '1'
    je Payment_OK         ; payment confirmed

    mov bx, offset parking_spots
    mov byte ptr [bx+di], 0 ; cancel reservation

    mov dx, offset payment_cancel_msg
    mov ah, 09h
    int 21h               ; payment cancelled message
    jmp ST_End

Payment_OK:
    inc total_cars_count  ; increase total cars count

    mov bx, offset spot_usage
    inc byte ptr [bx+di]  ; increase usage count for this spot

    mov dx, offset payment_success_msg
    mov ah, 09h
    int 21h               ; payment success message

ST_End:
    pop dx
    pop bx
    pop ax                ; restore registers
    ret
ShowTicketAndConfirm endp

;----------------------------------------------------
; Session Analytics Function 
; Count reserved spots by type: Normal, VIP, Disabled
SessionAnalytics proc
    push ax
    push bx
    push di               ; save registers

    xor di, di            ; set index to 0
    mov reserved_normal, 0
    mov reserved_vip, 0
    mov reserved_disabled, 0   ; initialize counters

SA_Loop:
    mov al, parking_spots[di] ; load spot status
    cmp al, 1
    jne SA_Next               ; if not reserved, skip

    mov bl, spot_priority[di] ; load spot type
    cmp bl, 0
    je SA_CountNormal         ; Normal spot
    cmp bl, 1
    je SA_CountVIP            ; VIP spot
    cmp bl, 2
    je SA_CountDisabled       ; Disabled spot
    jmp SA_Next

SA_CountNormal:
    inc reserved_normal       ; increment normal counter
    jmp SA_Next

SA_CountVIP:
    inc reserved_vip          ; increment VIP counter
    jmp SA_Next

SA_CountDisabled:
    inc reserved_disabled     ; increment Disabled counter

SA_Next:
    inc di
    cmp di, n_spots
    jb SA_Loop                ; loop until all spots checked

    mov dx, offset analytics_header
    mov ah, 09h
    int 21h                   ; print "Session Analytics:" header

    pop di
    pop bx
    pop ax                    ; restore registers
    ret
SessionAnalytics endp
;----------------------------------------------------

;----------------------------------------------------
; ShowSessionAnalytics proc
; Display session statistics and the most used spot
ShowSessionAnalytics proc
    push ax
    push bx
    push cx
    push di               ; save registers

    ; ===== Display analytics title =====
    mov dx, offset analytics_title
    mov ah, 09h
    int 21h

    ; ===== Display total cars today =====
    mov dx, offset total_cars_msg
    mov ah, 09h
    int 21h

    mov al, bl           ; currently BL is undefined here
    add al, '0'
    mov dl, al
    mov ah, 02h
    int 21h

    ; ===== Find most used spot =====
    mov cx, n_spots
    xor di, di           ; index = 0
    mov al, 0            ; max usage placeholder
    mov bl, 0            ; max index placeholder

FindMaxLoop:
    mov al, spot_usage[di]  ; load usage count of current spot
    mov bl, [max_usage]      ; load current max usage
    cmp al, bl
    jbe NextSpot             ; if current <= max, skip

    mov [max_usage], al      ; update max usage
    mov [max_index], di      ; update max index

NextSpot:
    inc di
    loop FindMaxLoop

    ; ===== Display most used spot =====
    mov dx, offset most_used_msg
    mov ah, 09h
    int 21h

    mov al, bl           ; currently BL still contains old max index
    add al, '0'
    mov dl, al
    mov ah, 02h
    int 21h

    pop di
    pop cx
    pop bx
    pop ax                ; restore registers
    ret
ShowSessionAnalytics endp
;----------------------------------------------------
