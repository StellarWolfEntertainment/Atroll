# _@Roll_ - A Simple Language for Dice-Based Programming
_@Roll_ is a simple and unique programming language designed for dice-based computations. This readme will guide you through the basics of writing programs in _@Roll_.

## Verbs
Verbs are the core actions in _@Roll_, and they allow you to perform various operations. Let's start by introducing the most fundamental verb: **Roll**.

### Roll
The **Roll** verb is used to generate random values by rolling one or more dice. A die declaration in _@Roll_ has the format `[Count]d[Sides]`, where `[Count]` represents the number of dice to roll, and [Sides] represents the number of sides on the die. This verb **MUST** initate a program, and can only be called the once, this verb generates the initial random values to be modified

### Add
The **Add** verb is used to add new rolls to the current set of rolls. It is followed by a die declaration in the same format as the **Roll** verb. This verb is useful for accumulating values and performing calculations based on the sum of rolled dice

### Drop
The **Drop** verb is used to remove extreme values (Highest or Lowest) from the results. It helps you filter out unwanted values from your set of results, allowing you to focus on specific outcomes.

### If
The **If** verb enables conditional execution of the next action based on whether or not a specified value is present in the rolled results. It is followed by an integer value declaration, and the subsequent action is executed only if the specified value is among the rolled results.

### While
The **While** verb allows you to repeatedly execute the next action while a specified value is present in the rolled results. Similar to the **If** verb, it is followed by an integer value declaration, and the subsequent action is executed repeatedly as long as the specified value appears in the results.

### Reroll
The **Reroll** verb replaces specific values in the results with new random values. It is followed by two integer value declarations: the value to be replaced and the number of sides on the new die. This verb is useful for modifying specific outcomes in your results.

### Mod
The **Mod** verb calculates the sum of all values in the results and adds a specified integer value to the total. It allows you to perform additional mathematical operations on your results.


## Examples

**NOTE**: This language does not support comments. The comments used in the following examples are used as demonstration only. The space before Nested Verbs is not necessary and is added only for ease of reading.

```atroll
Roll 4d6 // Roll 4 six-sided dice. All @Roll programs will begin with a line like this.
While 1 // So long as there is a 1 in the current set
 Reroll 1 6 // Reroll all 1's in the current set with a six-sided die
Drop Lowest // take the lowest roll in the set and discard it
```
This program is summed up as rolling 4d6 rerolling 1s and discarding the lowest roll.

```atroll
Roll 1d8 // Roll 1 eight-sided die
While 1 // So long as there is a 1 in the current set
 Reroll 1 8 // Reroll all 1's in the current set with an eight-sided die
Mod 10 // Summate the current set and add 10 to the result
```
This program rolls a singular eight-sided die rerolling 1s and adds 10 to the result

```atroll
Roll 1d6 // Roll 1 six-sided die
Add 1d4 // Add a four-sided die roll to the current set
```
This one rolls a six-sided die and adds the result of rolling a 4 sided die to it
