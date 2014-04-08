copy /Y ".\BlankServer\bin\*.dll" .\distribution\blankserver\bin\
copy /Y ".\BlankServer\*.as?x" .\distribution\blankserver\
copy /Y ".\BlankServer\ServerData.mdb" .\distribution\blankserver\ServerData.mdb

copy /Y ".\ReceipeBook\app.config" .\distribution\RecipeBook\RecipeBook.exe.config
copy /Y ".\ReceipeBook\bin\debug\*.exe" .\distribution\RecipeBook\
copy /Y ".\ReceipeBook\bin\debug\*.dll" .\distribution\RecipeBook\
copy /Y ".\ReceipeBook\RecipeData.mdb" .\distribution\RecipeBook
copy /Y ".\ReceipeBook\Subscriptions.xml" .\distribution\RecipeBook

pause
