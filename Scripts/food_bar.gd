extends Node2D

var food_progress = 0

func _ready():
	update()


func update():
	food_progress += 10
