# MainEntry
`StartApplication.cs` is the MainEntry of the game. It first Loads all assets, then, in its `Update` method,  checks if everything has been loaded. If no, then it waits for the next frame and checks again. If yes, it starts the game's workflow and stops itself to avoid unnecessary Update calls.

Loading Assets is done in Holders. Holders are the link between the code and the resources. They use the Addressable Package to load Assets asynchronously. Every Holders are called by the `GameInitializer` in 3 steps.

 **1. Initialize**

 - Creates all the dictionary, list, variable [...] needed to hold the
   loaded objects.

 **2. Register the Loaded Event**
 - Add the function to call to check if every assets of the holder has
   been loaded. The function `CurrentLoadingPercentage` returns **1** if everything is loaded.

 **3. Load Assets**
 Makes the actual loading with the `Addressables.LoadAssetAsync` method. When the assets is loaded, it calls an anonymous function to set the holder variables and increment the `currentNumberOfLoadedAssets`.

**Note:** Holders that loads Authoring Prefab (Prefab with Convert to Entity) may also use *BlobAssetStore*, which they dispose in a`OnDestroy` Method. *BlobAssetStore* are needed to load anything with Physics Shape and/or Physics Body on it. It makes a link between the entity and the PhysicsWorld. 