> This week’s question:
> Given a QWERTY keyboard grid and a remote control with arrows and a “select” button, write a function that returns the buttons you have to press to type a certain word. The cursor will always start in the upper left at the letter Q.
> 
> Example:
> 
> ```
> $ remoteControl('car')
> $ 'down, down, right, right, select, left, left, up, select, up, right, right, right, select'
> ```

[Original Newsletter Issue](https://buttondown.email/cassidoo/archive/if-you-hit-a-wrong-note-its-the-next-note-that/)

Sample run of this program:

```
Enter a word or enter ! to exit: car
down, down, right, right, select, left, left, up, select, up, right, right, right, select
Enter a word or enter ! to exit: !
```

Original pass had precedence of ANY VERTICAL > ANY HORIZONTAL:

```c#
while (Abs(verticalDifference) > 0)
{
    yield return verticalDifference > 0 ? "down" : "up";
    verticalDifference += verticalDifference > 0 ? -1 : 1;
}
    
while (Abs(horizontalDifference) > 0)
{
    yield return horizontalDifference > 0 ? "right" : "left";
    horizontalDifference += horizontalDifference > 0 ? -1 : 1;
}
```

The specific order of the control statements probably doesn't matter, but it meant that the output for __CAR__ didn't match the given output exactly, so code is to specifically emit controls in preference order of DOWN > LEFT > UP > RIGHT:

```c#
while (verticalDifference > 0)
{
    yield return "down";
    verticalDifference--;
}

while (horizontalDifference < 0)
{
    yield return "left";
    horizontalDifference++;
}

while (verticalDifference < 0)
{
    yield return "up";
    verticalDifference++;
}

while (horizontalDifference > 0)
{
    yield return "right";
    horizontalDifference--;
}
```