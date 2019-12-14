A Voxel Game Made With Unity (2020.1.0a14)
========================================

This is a Voxel game Like Minecraft, which is based on Unity's Perlin Noise (Still Unfinished)

You may use this project freely.

What's Implemented?
------------------
- Added SEM.cs which is short for (Static Extension Methods)
- Added Seed Translation (look for GetSeed() in SEM.cs) to translate string letters to Int32
- Created A Solid Player Physics And Player Controller (Not Buggy At All)
- Added A Chunk System To Improve Performance (I was stupid to the point where i render every block as a meshRenderer but we learn from our mistakes)
- Added Seeded World Generation based on Transform Position (Not Spiral Which may cause a tiny performance issues)
- Added Support for Texture Importing to Simulate texture packs And UV Mapping
(Texture packs are in the StreamingAssets path)
- Added Support for MeshCollider (meshColliders are laggy and buggy, i will change that in the future)
- Added Render Distance Option To The World.cs class which gets called on Awake() Only, you can change it to Update when it Render Distance Option changes :wink:
- Added Slobe climbing (Very cool for Stairs And Ladders)
- Chunks are now Generated Based on the block facing which gives extra 110+ FPS!
- Multithreaded Chunk Building, makes you see the world being created :yum:
