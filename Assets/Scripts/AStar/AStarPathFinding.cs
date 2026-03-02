using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Class untuk implementasi algoritma A* Pathfinding
// A* adalah algoritma untuk mencari jalur terpendek dari titik A ke B
public class AStarGroundPathfinding : MonoBehaviour
{
    // Reference ke Tilemap yang digunakan untuk navigasi
    // Tilemap berisi tile-tile yang bisa dilalui
    public Tilemap groundTilemap;

    // Dictionary untuk menyimpan node yang sudah dibuat
    // Key: Vector3Int (posisi cell), Value: Node object
    // Digunakan untuk menghindari membuat node duplikat untuk cell yang sama
    Dictionary<Vector3Int, Node> nodeCache = new Dictionary<Vector3Int, Node>();

    // ================= FUNGSI UTAMA A* =================
    // Fungsi untuk mencari jalur dari startCell ke targetCell
    // Return: List posisi cell yang membentuk jalur (dari start ke target)
    //         atau null jika tidak ada jalur
    public List<Vector3Int> CariJalur(Vector3Int startCell, Vector3Int targetCell)
    {
        // Bersihkan cache node dari pencarian sebelumnya
        nodeCache.Clear();

        // Buat node untuk titik start dan target
        Node startNode = GetNode(startCell);
        Node targetNode = GetNode(targetCell);

        // OpenSet = list node yang akan diperiksa
        // Berisi node-node yang masih kandidat untuk dijelajahi
        List<Node> openSet = new List<Node>();
        
        // ClosedSet = set node yang sudah diperiksa
        // Node di sini sudah selesai diproses dan tidak akan diperiksa lagi
        HashSet<Node> closedSet = new HashSet<Node>();

        // Tambahkan node start ke openSet sebagai titik awal
        openSet.Add(startNode);

        // Loop selama masih ada node yang perlu diperiksa
        while (openSet.Count > 0)
        {
            // STEP 1: Pilih node dengan F Cost terendah dari openSet
            // Node pertama di list dianggap sebagai current (awalnya)
            Node current = openSet[0];

            // Loop semua node di openSet untuk mencari yang terbaik
            foreach (Node node in openSet)
            {
                // Pilih node jika:
                // - F Cost lebih rendah, ATAU
                // - F Cost sama tapi H Cost lebih rendah (lebih dekat ke target)
                if (node.fCost < current.fCost ||
                    (node.fCost == current.fCost && node.hCost < current.hCost))
                    current = node;
            }

            // STEP 2: Pindahkan node terpilih dari openSet ke closedSet
            openSet.Remove(current);   // Hapus dari openSet
            closedSet.Add(current);     // Tambahkan ke closedSet

            // STEP 3: Cek apakah sudah sampai target
            if (current.cellPos == targetNode.cellPos)
            {
                // Jalur ditemukan! Retrace path dari target ke start
                return RetracePath(startNode, targetNode);
            }

            // STEP 4: Periksa semua tetangga (neighbor) dari current node
            foreach (Vector3Int neighborPos in GetNeighbors(current.cellPos))
            {
                // Dapatkan node tetangga
                Node neighbor = GetNode(neighborPos);

                // Skip tetangga jika sudah ada di closedSet
                // (sudah diperiksa, tidak perlu diperiksa lagi)
                if (closedSet.Contains(neighbor)) continue;

                // Hitung G Cost baru jika melewati current node
                // G Cost = jarak dari start ke current + 10 (cost 1 langkah)
                int newCost = current.gCost + 10;
                
                // Update neighbor jika:
                // - Jalur baru lebih pendek (newCost < gCost lama), ATAU
                // - Neighbor belum pernah ada di openSet (belum pernah diperiksa)
                if (newCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    // Update G Cost dengan nilai baru (jalur lebih pendek)
                    neighbor.gCost = newCost;
                    
                    // Hitung H Cost (estimasi jarak ke target)
                    neighbor.hCost = GetDistance(neighbor.cellPos, targetNode.cellPos);
                    
                    // Hitung F Cost (G + H)
                    neighbor.HitungFCost();
                    
                    // Set parent agar bisa retrace jalur nanti
                    neighbor.parent = current;

                    // Jika neighbor belum ada di openSet, tambahkan
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        // Jika loop selesai tapi tidak return = tidak ada jalur yang ditemukan
        return null;
    }

    // ================= HELPER FUNCTIONS =================
    
    // Fungsi untuk mendapatkan atau membuat node dari cache
    // Menghindari membuat node duplikat untuk cell yang sama
    Node GetNode(Vector3Int cell)
    {
        // Jika node untuk cell ini belum ada di cache
        if (!nodeCache.ContainsKey(cell))
        {
            // Buat node baru dan simpan ke cache
            nodeCache[cell] = new Node(cell);
        }

        // Return node dari cache
        return nodeCache[cell];
    }

    // Fungsi untuk mendapatkan semua tetangga (neighbor) dari sebuah cell
    // Return: List posisi cell tetangga yang bisa dilalui
    List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        // List untuk menyimpan posisi tetangga
        List<Vector3Int> neighbors = new List<Vector3Int>();

        // Array berisi 4 arah: kanan, kiri, atas, bawah
        // (tidak ada diagonal, hanya 4 arah cardinal)
        Vector3Int[] dirs =
        {
            Vector3Int.right,   // (1, 0, 0) - ke kanan
            Vector3Int.left,    // (-1, 0, 0) - ke kiri
            Vector3Int.up,      // (0, 1, 0) - ke atas
            Vector3Int.down     // (0, -1, 0) - ke bawah
        };

        // Periksa setiap arah
        foreach (var dir in dirs)
        {
            // Hitung posisi cell tetangga
            Vector3Int next = cell + dir;
            
            // Jika ada tile di posisi tersebut (bisa dilalui)
            if (groundTilemap.HasTile(next))
            {
                // Tambahkan ke list tetangga
                neighbors.Add(next);
            }
        }

        // Return list semua tetangga yang valid
        return neighbors;
    }

    // Fungsi untuk menghitung jarak Manhattan antara 2 cell
    // Manhattan Distance = |x1-x2| + |y1-y2| (tidak ada diagonal)
    // Dikali 10 untuk konsistensi dengan G Cost
    int GetDistance(Vector3Int a, Vector3Int b)
    {
        // Hitung selisih absolut posisi X dan Y, lalu dikali 10
        return 10 * (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y));
    }

    // Fungsi untuk retrace/melacak balik jalur dari end ke start
    // Mengikuti parent dari setiap node sampai kembali ke start
    // Return: List posisi cell yang membentuk jalur (urutan dari start ke end)
    List<Vector3Int> RetracePath(Node start, Node end)
    {
        // List untuk menyimpan jalur
        List<Vector3Int> path = new List<Vector3Int>();
        
        // Mulai dari node end (target)
        Node current = end;

        // Loop mundur mengikuti parent sampai ke start
        while (current != start)
        {
            // Tambahkan posisi cell current ke path
            path.Add(current.cellPos);
            
            // Pindah ke parent (node sebelumnya)
            current = current.parent;
        }

        // Saat ini path urutan: end → start
        // Reverse agar urutannya: start → end
        path.Reverse();
        
        // Return jalur final
        return path;
    }
}