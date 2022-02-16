> This week’s question:
> Given a QWERTY keyboard grid and a remote control with arrows and a “select” button, write a function that returns the buttons you have to press to type a certain word. The cursor will always start in the upper left at the letter Q.
> 
> Example:
> 
> ```
> $ remoteControl('car')
> $ 'down, down, right, right, select, left, left, up, select, up, right, right, right, select'
> ```

Sample run of this program:

```
Enter a word or enter ! to exit: car
down, right, right, select, left, left, up, up, select, up, up, right, right, right, select
Enter a word or enter ! to exit: !
```

The specific order of the control statements probably doesn't matter, but it made testing a bit annoying, so code is to specifically emit controls in preference order of DOWN > LEFT > UP > RIGHT:
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