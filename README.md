# Card Sorting

Compile with Visual Studio or MSBuild.

Then run with CardsDemo.bat --random/--sorted/ect.
(it sets up the windows terminal to print unicode chars )

You can also run the Unit Tests in Visual Studio.

I tried to not go overboard/overcomplicated.

The last time I did this sort of challenge I wrote a small
game of Durak to demonstrate Card Comparison(which by its
nature may not be as simple as suit and card order, and is specific 
to the game), 
but Tony implied this might be overkill. 


If I was doing a larger personal project I'd add a Maybe
struct(value type is preferable since Maybe shouldn't be null)
and annotate every parameter not null, plus add a number of usefull
extension methods including chunking ienumerable, FirstOrMaybe, ect.

For a group project I'd be careful to tow to the house style but I 
would encourage reusable libraries, extension methods, 
and linq over foreach loops most of the time.

I added 2-10 to the suit enum to avoid casting to int all over the place.
If I was to redo this I might switch to constants instead, 
C# enums can be a bit cucumbersome.
