extends ProgressBar

#@onready var progress_bar = $ProgressBar
var food_progress = 0 : set = _set_food_count

func _ready():
	print("ready")
	init_food_state()


func _set_food_count(new_food_count):
	print("food count changed")
	var prev_count = food_progress
	var current_food_count = food_progress + new_food_count
	food_progress = min(max_value, new_food_count)
	value = food_progress


	if food_progress >= 70:
		pass
		#foodMinimum.emit()


	if food_progress >= 90:
		pass


	if food_progress == 100:
		pass


func init_food_state():
	max_value = 100
	value = food_progress
