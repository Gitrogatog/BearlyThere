extends Node2D

var state = "uneaten"
var pickedUp = false
signal food_bar

# Called when the node enters the scene tree for the first time.
func _ready():
	GlobalEvents.StartLevel.connect(on_reset)

func on_reset():
	state = "uneaten"

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	if state == "uneaten":
		$AnimatedSprite2D.play("uneaten")
	if state == "ate":
		$AnimatedSprite2D.play("ate")


func on_collision(body: Node2D):
	print("collided")
	if body.has_meta("IsPlayer") and state != "ate":
		print("player")
		state = "ate"
		food_bar.emit()
	else:
		print("notplayer")


func on_collect():
	if state == "ate":
		GlobalEvents.CollectFood.emit()
#food_bar.emit(food)
