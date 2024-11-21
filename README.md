# UIForSomethingMobileMagicGame
_a long, long time ago, in a Unity far, far away..._

__Hi everyone. First attempt to do something in Unity, first attempt to create README-file. Let's see what happens.__

## About project
It's a small mobile project that mainly contains user interface elements. Project have a pie menu, buttons and joysticks. ___It's not example "how you need to do"!___

## What can we do as users?
The user interface is divided into left and right sides. 

Left side:
+ joystick

Rightside:
+ button
+ joystick
+ pie menu

Left joystick responsible for movement, right joystick responsible for direction(spell). 

Right side contains of three statuses:
+ "Off" - you see only button. Pressing button goes to next status(press and release)
+ "Cast"- added pie menu. Pressing button goes to next status(only press)
+ "Direction" - you see only joystick. Releasing joystick(not in center) goes to next status

## Spell list or idea base

### ELEMENTS(code) - Spell Name(damage type):
+ RGB(5) -	Beam(area damage)
+ RRR(0) -	Damage Boost(buff)
+ RRG(1) - Fire Row(solo damage)
+ RRB(4) - Fire whirlwind(area damage)
+ GGG(3) - Shield(buff)	
+ GGR(2) - Rock Block(solo damage)
+ GGB(6) - Meteorite(area damage)	
+ BBB(12) - Mana Regeneration	(buff)
+ BBR(8) - Ice Arrows(solo damage)
+ BBG(9) - Frost Blast(area damage)

### Elements and code:
+ R - Red color in pie menu (code:0)
+ G - Green color in pie menu (code:1)
+ B - Blue color in pie menu (code:4)

You might say: Stas what are these codes for? All sums of elements with such parameters give different values. This method easily identifies a spell. This won't work for 4 elements

### Damage type affects direction display:
+ area damage - circle direction
+ solo damage - rectangle direction
+ buff - nothing

## END?

Yes, that's all. If you found any errors, have any interesting ideas or want to do something together, [call me maybe](https://www.youtube.com/watch?v=fWNaR-rxAic)

## Communications 

[Telegram](https://t.me/stasyancho)

[Linkedin](https://www.linkedin.com/in/stanislav-golovin-2b1496241/)
