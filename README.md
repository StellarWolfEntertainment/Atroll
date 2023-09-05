# _@Roll_ - A Simple Language for Dice-Based Programming
_@Roll_ is a simple and unique programming language designed for dice-based computations. This readme will guide you through the basics of writing programs in _@Roll_.

## Verbs
Verbs are the core actions in _@Roll_, and they allow you to perform various operations. Let's start by introducing the most fundamental verb: **Roll**.

### Roll **(Roll must be the first action in a program)**
- Description: This verb represents rolling dice.
- Usage: It calculates random values based on the dice specified and adds them to the previous results.
- Example: `Roll 2d6`

### Drop
Description: This verb represents dropping extreme values from the previous results.

Usage: It removes either the highest or lowest values from the previous results based on the rule specified.

Example: `Drop Lowest`

### Reroll
Description: This verb represents rerolling specific values in the previous results.

Usage: It rerolls values that match the condition and replaces them with new random values.

Example: `Reroll <2 d6`

### If
Description: This verb represents conditional branching.

Usage: It evaluates a condition and executes a child statement if the condition is met.

Example: `If <2`

### While
Description: This verb represents a loop with a condition.

Usage: It repeatedly executes a child statement as long as the condition is true.

Example: `While <3`

### For
Description: This verb represents a for loop.

Usage: It iterates a specified number of times, executing a child statement each iteration.

Example: `For 5`

### Add, Sub, Mul, Div
Description: These verbs represent basic mathematical operations.

Usage: They perform addition, subtraction, multiplication, or division on the previous results.

Example: `Add 10`

## Examples

**NOTE**: This language does not support comments. The comments used in the following examples are used as demonstration only. The space before Nested Verbs is not necessary and is added only for ease of reading.

```atroll
Roll 4d6 // Roll 4 six-sided dice. All @Roll programs will begin with a line like this.
While <=2 // So long as there is a 2 or less in the current set
 Reroll <=2 d6 // Reroll all 2's and 1's in the current set with a six-sided die
Drop Lowest // take the lowest roll in the set and discard it
```
This program is summed up as rolling 4d6 rerolling 1s and 2s and discarding the lowest roll.

```atroll
Roll 1d8 // Roll 1 eight-sided die
While 1 // So long as there is a 1 in the current set
 Reroll 1 d8 // Reroll all 1's in the current set with an eight-sided die
Add 10 // Summate the current set and add 10 to the result
```
This program rolls a singular eight-sided die rerolling 1s and adds 10 to the result

```atroll
Roll 1d6
Roll 1d4
```
This one rolls a six-sided die and adds the result of rolling a 4 sided die to it
