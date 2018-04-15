MRM Brand .NET Exercise
======

Intro
------
A simple .NET Web API example service suite using the Entity Framework, seeded with an included json file.

Created for the MRM Brand Technical Exercise, avaliable [here](https://github.com/MRMBRAND/MRMDotNetTest)


Issues
------
###aka Things I'd fix with more time###
 
 * In hindsight I should of split the _category_ field out of the `Product` model in to it's own entity. Would make for a better data structure at the back-end.
 * The unit test suite isn't very robust. Should of made a greater number of more granular tests rather than "does this method work" tests.
 * I ended up with an untidy testing/mocking system which I'm not hugely happy with. Could be improved/refactored countless ways.
 * Also on the test front I did use [Fiddler](https://www.telerik.com/fiddler) to externally query the API, but surely there is some clever VS extension that would allow you to automate this?
 * Didn't dispose of any objects anywhere, which is probably super irresponsible programming.
 * The `DBContext` class mostly handles the primary key ids on it's own, yet I've got them in the `Product` model for ease and testing. This is far from consistent.
 
 ...and finally...
 
 * Could not get the `ProductsController.Put()` associated test method to work. At. All. Grumble grumble bet its something obvious grumble grumble.