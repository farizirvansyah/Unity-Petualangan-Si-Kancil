using UnityEngine;

// Class yang merepresentasikan satu node/titik dalam grid untuk algoritma A*
// Setiap node menyimpan informasi posisi dan cost untuk pathfinding
public class Node
{
    // Posisi node dalam koordinat cell/grid (Vector3Int)
    // Contoh: (5, 3, 0) artinya kolom 5, baris 3
    public Vector3Int cellPos;
    
    // G Cost = jarak dari node start ke node ini
    // Semakin jauh dari start, semakin besar gCost
    // Dalam implementasi ini, setiap langkah = 10
    public int gCost;
    
    // H Cost (Heuristic Cost) = estimasi jarak dari node ini ke node target
    // Dihitung menggunakan Manhattan Distance (jarak horizontal + vertikal)
    // Contoh: dari (2,2) ke (5,4) = |5-2| + |4-2| = 3 + 2 = 5 → hCost = 50
    public int hCost;
    
    // F Cost = G Cost + H Cost
    // Ini adalah total cost yang digunakan A* untuk memilih node terbaik
    // Node dengan fCost terendah akan dipilih terlebih dahulu
    public int fCost;
    
    // Reference ke node parent (node sebelumnya dalam jalur)
    // Digunakan untuk retrace/melacak balik jalur dari target ke start
    public Node parent;

    // Constructor - dipanggil saat membuat node baru
    // Parameter: cellPos = posisi cell dari node ini
    public Node(Vector3Int cellPos)
    {
        // Simpan posisi cell
        this.cellPos = cellPos;
    }

    // Fungsi untuk menghitung F Cost
    // F = G + H (total cost = jarak dari start + estimasi jarak ke target)
    public void HitungFCost()
    {
        fCost = gCost + hCost;
    }
}