extends Node2D

var state = "uneaten"
var pickedUp = false
#@onready var progress_bar = $ProgressBar
var food_value = 0
#Signal  foodProgressBar(foodValue)

# Called when the node enters the scene tree for the first time.
func _ready():
	#progress_bar.init_food_state(food_value)
	#var progress_bar = get_node("../MainUI/foodProgressBar")
	GlobalEvents.StartLevel.connect(on_reset)
	#	$ProgressBar.init_food_state(food_value)

func on_reset():
	var progress_bar = get_node("../Main UI/foodProgressBar")
	progress_bar.init_food_state()
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
		var progress_bar = get_node("../Main UI/foodProgressBar")
		progress_bar._set_food_count(10)
	else:
		print("notplayer")


func on_collect():
	if state == "ate":
		food_value += 10
		#progress_bar.food_progress += 10
		GlobalEvents.CollectFood.emit()


