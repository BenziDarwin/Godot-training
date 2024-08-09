using Godot;

public partial class Main : Node
{
	// Exported PackedScene variable for MobScene
	[Export]
	public PackedScene MobScene { get; set; }

	private int _score;

	// Called when the node is added to the scene.
	public override void _Ready()
	{
		NewGame();
	}

	// Function to handle game over logic.
	public void GameOver()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
	}

	// Function to start a new game.
	public void NewGame()
	{
		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();
	}

	// Called when ScoreTimer times out.
	private void OnScoreTimerTimeout()
	{
		_score++;
	}

	// Called when StartTimer times out.
	private void OnStartTimerTimeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	// Called when MobTimer times out.
	private void OnMobTimerTimeout()
	{
		// Create a new instance of the Mob scene.
		Mob mob = (Mob)MobScene.Instantiate();

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
