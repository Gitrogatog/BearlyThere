extends Node2D

var state = "uneaten"
var pickedUp = false

# Called when the node enters the scene tree for the first time.
func _ready():
	pass

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if state == "uneaten":
		$AnimatedSprite2D.play("uneaten")
	if state == "ate":
		$AnimatedSprite2D.play("ate")


func on_collision(body: Node2D):
	if body.has_meta("player"):
		state == "ate"