tool
extends EditorPlugin

# https://docs.godotengine.org/en/stable/tutorials/plugins/editor/making_main_screen_plugins.html

const MainScreen := preload("res://addons/Dungeon/MainScreen/MainScreen.tscn")
const LevelControllerScript: CSharpScript = preload("res://Scenes/Controller/LevelController.cs")
const LevelControllerIcon := preload("res://addons/Dungeon/DungeonPlugin.svg")

var mainScreen: HBoxContainer

func _enter_tree() -> void:
    print("==============")
    print("Dungeon Plugin")
    print("==============")

    add_custom_type("LevelController", "Spatial", LevelControllerScript, LevelControllerIcon)

    mainScreen = MainScreen.instance()
    get_editor_interface().get_editor_viewport().add_child(mainScreen)
    make_visible(false)

func _exit_tree() -> void:
    if mainScreen:
        mainScreen.queue_free()
        mainScreen = null
    remove_custom_type("LevelController")

func has_main_screen() -> bool:
    return true

func handles(object: Object) -> bool:
    print ("Test: ", object)
    return object.is_class("LevelController")

func make_visible(visible: bool) -> void:
    if mainScreen:
        mainScreen.visible = visible

func get_plugin_name() -> String:
    return "Dungeon"

func get_plugin_icon() -> Texture:
    return LevelControllerIcon
