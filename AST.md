# Language Structure
**Statements**: Each line in the code represents one statement.

## Statement Formats:

Each statement follows one of two formats:
- Verb Modifier
- Verb Modifier1 Modifier2

Verbs and Modifiers:

The choice of verb determines the allowed modifiers for a statement.

Verbs and their corresponding modifiers:
- Roll: Requires either a Die or a PartialDie.
- Drop: Requires an Extreme (either Highest or Lowest).
- Reroll: Requires either an EqualityInteger or an Integer (treated as Equals) and a PartialDie.
- While and If: Require either an EqualityInteger or an Integer (treated as Equals) to determine execution conditions.
- For: Requires an Integer to specify the number of iterations.
- Add, Sub, Mul, and Div: Require an Integer for mathematical operations.

## Examples

```
1. Roll Die
2. Drop Highest
3. Reroll 3 d6
4. While 5 > 0
5. If 10 Equals
6. For 3
7. Add 5
```
