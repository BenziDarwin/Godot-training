using Godot;

public partial class Main : Node
{
	// Exported PackedScene variable for MobScene
	[Export]
	public PackedScene MobScene { get; set; }

	private int _score;
	
	// Function to handle game over logic.
	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetNode<HUD>("HUD").ShowGameOver();
		  GetNode<AudioStreamPlayer>("Music").Stop();
	GetNode<AudioStreamPlayer>("DeathSound").Play();
	}

	// Function to start a new game.
	public void NewGame()
	{
		GetNode<AudioStreamPlayer>("Music").Play();
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);
		_score = 0;
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("PlayerPosition");
		player.Start(startPosition.Position);
		GetNode<Timer>("StartTimer").Start();
		var hud = GetNode<HUD>("HUD");
hud.UpdateScore(_score);
hud.ShowMessage("Get Ready!");
	}

	// Called when ScoreTimer times out.
	private void OnScoreTimerTimeout()
	{
		_score++;
		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	// Called when StartTimer times out.
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	// Called when MobTimer times out.
	// We also specified this function name in PascalCase in the editor's connection window.
// We also specified this function name in PascalCase in the editor's connection window.
private void OnMobTimerTimeout()
{
	Mob mob = MobScene.Instantiate<Mob>();

	// Choose a random location on Path2D.
	var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
	mobSpawnLocation.ProgressRatio = GD.Randf();

	// Set the mob's direction perpendicular to the path direction.
	float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

	// Set the mob's position to a random location.
	mob.Position = mobSpawnLocation.Position;

	// Add some randomness to the direction.
	direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
	mob.Rotation = direction;

	// Choose the velocity.
	var velocity = new Vector2((float)GD.RandRange(150.0, 250.0), 0);
	mob.LinearVelocity = velocity.Rotated(direction);

	// Spawn the mob by adding it to the Main scene.
	AddChild(mob);
}
	
}



