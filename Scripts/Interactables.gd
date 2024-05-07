extends TileMap

@onready var mapscene_GUI = get_node("../GUI")

var loot_area = preload("res://Scenes/subScenes/node_2d.tscn")


# Called when the node enters the scene tree for the first time.
func _ready():
	var used_chest_tiles = get_used_cells_by_id(-1, 2, Vector2i(1, 0))
	for tile in used_chest_tiles:
			var loot_area_instance = loot_area.instantiate()
			loot_area_instance.position = to_global(tile)
			add_child(loot_area_instance)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
