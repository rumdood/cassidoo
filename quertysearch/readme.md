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

## Random Notes Nobody Cares About

#### Calculating Directions
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

#### Obsessive Optimizations
##### (or how you can tell I've done too many interviews lately)

I had  the code in place at one point to not load the entire KeyMap (dictionary of keys/positions) on first run, but rather to build the KeyMap on demand. If a character was not in the keymap, find the last key indexed into the map and then iterate from there until you find the key. This means you could have a theoretical, insanely large keyboard and potentially avoid iterating through all of them.

```c#
private static Coordinate MaxIndexedKey = new MaxIndexedKey(0, 0);
...
// load the dictionary as needed
// this is a silly optimization for 26 elements. It would be simpler and fine to just preload the entire
// dictionary at the outset. But now you can have a keyboard of UNLIMITED KEYS!
// This is why I'm in therapy.
if (!KeyMap.TryGetValue(c, out var target))
{
    for (var rowIndex = MaxIndexedKey.Row; rowIndex < Keys.Length; rowIndex++)
    {
        for (var columnIndex = MaxIndexedKey.Column; columnIndex < Keys[rowIndex].Length; columnIndex++)
        {
            KeyMap[Keys[rowIndex][columnIndex]] = new Coordinate(rowIndex, columnIndex);

            if (Keys[rowIndex][columnIndex] == c)
            {
                MaxIndexedKey.Column = columnIndex;
                break;
            }
        }

        if (KeyMap.ContainsKey(c))
        {
            MaxIndexedKey.Row = rowIndex;
            break;
        }

        MaxIndexedKey.Column = 0;
    }
    

    target = MaxIndexedKey;
}
```