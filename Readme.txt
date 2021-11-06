Echo4GSS
Adds Echo to SPC files generated
by SNES GSS

SNESGSS is a music tracker for SNES.

! NOTE !
This won't work with SPC files
from any other source. Only SNESGSS.
SNESGSS produces very specifically 
organized SPC files that this app
knows how to modify. Other kinds
of SPC files will just break if you
try to use them with this app.

Three uses of this app:
1. For musicians who write songs in
   SNESGSS and want to add echo
   to the SPC files exported from
   SNESGSS.
2. For game developers who want to
   use SNESGSS to write songs for
   the game. This app will make a
   byte array of all the echo
   parameters.
3. For musicians who want to use
   SNESGSS to produce BRR files
   for another system, such as
   AddmusicK.
   
   

Use 1 - Add Echo to an SPC
!! only SPC files made by SNESGSS

In SNESGSS, write a song.
Then, select song/export SPC.

1. in Echo4GSS, open the SPC.
2. change the Echo settings
3. save the SPC. It will have echo.
4. Play it in MESEN-S, and emulator
   that can also play SPC files.



Use 2 - Game Developer
(ca65 asm SNESdev)

1. Make songs in snesgssQv2.exe or later
   https://github.com/nesdoug/SNES_13/MUSIC
2. Export the spc700.bin and song bin files
3. See SNES_13 example code to make it all
   work in asm.
   You need music.asm from the MUSIC folder.
   Make sure it says VERSION 5.
4. in Echo4GSS, type the settings in, then
   save the settings file. .incbin this
   file to the asm project, with a label
   above it.
5. After SPC_Init, you need to pass a
   pointer to SPC_All_Echo to the Echo 
   settings label.
   lda #.loword(EchoSet)
   ldx #^EchoSet
   jsl SPC_All_Echo
6. Play the song. It should have echo.

(SNES_13, mainB.asm has an example of use)

Optionally, before step 4...
   From SNESGSS, you can Song/Export SPC 
   each song. Load that SPC to Echo4GSS, 
   type the echo settings, save the SPC.
   Echo4GSS does some error tests to make 
   sure the Start Address and Delay Size 
   won't cause errors. It can only tell 
   you that if you load the SPC file.
   You can then save the SPC file to 
   listen to, to see if you like those
   settings for the song. However, this
   SPC file will not be included with 
   the Game Project... it is only for
   listening to. The settings file is
   what we want for gamedev.



Use 3 - Extract BRR Files

1. in SNESGSS, import WAV files
2. song/export SPC.
3. put the SPC file in the folder
   where you want your BRR files
4. in Echo4GSS, open the SPC. 
5. BRR/extract 1 or BRR/extract all



Notes-
to save only 1 BRR file, you need to first 
select the number at the bottom right.

You can export an info text file, which has
all the details of each BRR including the
ADSR values.
...also in the txt file is the location of
each brr file in the ARAM. I had an idea
that you could use the last BRR file as a
sound effect, unlooped, and you could 
overwrite that one brr with another sound
effect (between levels), and it should play
the same as the previous one. I haven't
tried this, but this info file would give
you the location of the last BRR file.
Using SPC_Load_Data is a bit complicated
and slow. There is an example of its use
in the commented out section in main.asm
in the SNES_13 example.



Tips on Settings
- Echo start must be >= the "End of SPC" #
  (that is a SNESGSS thing, which loads the
  ARAM from beginning to end)
- I've heard people say that a delay of 4-5
  sounds best, but I've seen games use 2-3
  and that sounded fine.
- Each value of delay uses $800 bytes of ARAM,
  so keep that in mind when writing the song
  and leave some blank space at the end
- Main volume should be a little less than max,
  around $60-70
- Echo volume should be half that, maybe $20-40
- Feedback of $40-60 is good.



Unrelated to Echo... when you make samples in
SNESGSS, don't put everything at full volume.
Set each channel volume to 50%, maybe less (25%). 
At 100% it is very easy to have multiple notes on 
different channels combine to cause clipping, 
which sounds like noise/distortion.



Examples of FIR filter settings...
https://sneslab.net/wiki/FIR_Filter


Values $80-FF are negative numbers. (ff = -1)
(that would flip the output wave)
Main Volume can't be negative, 0-7f only.
(another quirk of the SNESGSS code)

Sorry, I didn't support different L and R values.


I believe this works will all versions of SNESGSS
for modifying SPC files (as of Nov 2021).

