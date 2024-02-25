# GreedyGame_AyushShreshthi

In this project, 
   1. A JSON file for detailed ui elements
   2. UITemplateGenerator.cs class -
          it consists serialization and deserialization for ui object, code for an editor tool to load save ui elements from our json file,
          in this tool you can add any gameobject with certain properties and update our JSON file.

   3. UIHierarchyInstantiator.cs class -
                                a monobehaviour class which can be attached into scene to instantiate uielements from our json file. it has fields for parent transform
                                and a canvas prefab to instantiate 2d ui elements(in this example project).

   5. UIcustomization.cs -
                                this is an editor tool to customize any gameobject from our scene. user can drag and drop any gameobject he/she wants, change new properties
                                for it, and update our json file as well.
  
