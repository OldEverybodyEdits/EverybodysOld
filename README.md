# EverybodysOld
A framework that takes the Old EE protocol and integrates it into an easy-to-use 

# Useage
Simply include EverybodysOld into your project, as well as the default PlayerIOClient that comes with it.

Then, you need to create the OldBot to use.

`OldBot bot = new OldBot();`

After doing so, you can make it join a world by simply

`bot.JoinWorld(0, 0);`

You join the room at 0, 0.

Now that you've joined the world, wait untill you've joined.

`while(!bot.Connected) { }`

You can now apply your own OnMessages to recieve OldMessages, as well as OnDisconnects.

`bot.OnMessage += bot_OnMessage;
bot.OnDisconnect += bot_OnDisconnect;

public void OnMessage(object sender, OldMessage e)
{
	
}

public void OnDisconnect(object sender, string message)
{
	
}`

And that's it. You can use the default values such as the Player List

`bot.Players`

filled with the OldPlayer class, or the World

`bot.World[x, y]`

that returns a Block.

# Sending

Using your bot,

`bot.PlaceBlock(X, Y, Block.Empty);
bot.ChangeFace(Smiley.Smiley);
bot.MoveToBlock(2, 2);
bot.MoveTo(32, 32);
bot.Move(32, 32, 0, 0, 0, 0, 0, 0);`

These are your possible functions you can use.