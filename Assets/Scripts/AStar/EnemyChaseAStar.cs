using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Script untuk mengontrol perilaku musuh yang dapat berpatroli, mengejar, dan menyerang player
// menggunakan algoritma A* pathfinding
public class EnemyChaseGroundAStar : MonoBehaviour
{
    // ================= STATE ENUM =================
    // Enum untuk menentukan state/kondisi musuh saat ini
    public enum EnemyState
    {
        Patrol,  // Musuh berpatroli mengikuti waypoint
        Chase,   // Musuh mengejar player
        Attack   // Musuh menyerang player
    }

    // ================ STATE =================
    // Variabel untuk menyimpan state musuh saat ini, default = Patrol
    EnemyState state = EnemyState.Patrol;

    // ================= AUDIO CONTROLLER =================
    // Reference ke komponen audio untuk efek suara musuh
    EnemySFX audioController;

    // ================= VARIABLES =================
    // Reference ke transform player (target yang akan dikejar)
    public Transform player;
    
    // Reference ke sistem pathfinding A*
    public AStarGroundPathfinding pathfinding;
    
    // Reference ke tilemap ground (untuk navigasi)
    public Tilemap groundTilemap;
    
    // Reference ke animator untuk animasi musuh
    public Animator enemyAnimator;
    
    // Reference ke sprite renderer untuk flip sprite
    public SpriteRenderer enemySprite;

    [Header("Settings")]
    // Array titik-titik patroli yang akan diikuti musuh
    public Transform[] patrolPoints;
    
    // Kecepatan gerak musuh
    public float speed = 3f;
    
    // Jarak deteksi player (radius lingkaran deteksi)
    public float detectionRadius = 6f;
    
    // Jarak serangan (jika player dalam jarak ini, musuh akan menyerang)
    public float attackRange = 1f;
    
    // Damage yang diberikan per serangan
    public int attackDamage = 1;
    
    // Cooldown antara serangan (dalam detik)
    public float attackCooldown = 1f;
    
    // Waktu tunggu di setiap waypoint sebelum lanjut ke waypoint berikutnya
    public float waypointWaitTime = 1f;

    // Internal Variables
    // Waktu serangan terakhir (untuk cooldown)
    float lastAttackTime;
    
    // List yang menyimpan jalur A* dari posisi saat ini ke target
    List<Vector3Int> path;
    
    // Index node saat ini yang sedang dituju dalam jalur A*
    int pathIndex;
    
    // Index waypoint patroli yang sedang dituju
    int patrolIndex;
    
    // Flag apakah musuh sedang menunggu di waypoint
    bool isWaitingAtWaypoint;
    
    // Timer untuk menghitung waktu tunggu di waypoint
    float waypointWaitTimer;
    
    // Vector untuk menyimpan arah gerakan terakhir (untuk flip sprite)
    Vector3 lastMoveDirection;

    // Dipanggil saat GameObject pertama kali dibuat
    void Awake()
    {
        // Ambil komponen EnemySFX yang ada di GameObject yang sama
        audioController = GetComponent<EnemySFX>();
    }

    // Dipanggil setiap frame
    void Update()
    {
        // Hitung jarak antara musuh dan player
        float distance = Vector2.Distance(
            transform.position,  // Posisi musuh
            player.position      // Posisi player
        );

        // Tentukan state baru berdasarkan jarak
        EnemyState newState;

        // Jika player dalam jarak serangan → Attack
        if (distance <= attackRange)
            newState = EnemyState.Attack;
        // Jika player dalam jarak deteksi → Chase
        else if (distance <= detectionRadius)
            newState = EnemyState.Chase;
        // Jika player jauh → Patrol
        else
            newState = EnemyState.Patrol;

        // Jika state berubah
        if (newState != state)
        {
            // Update state
            state = newState;
            
            // Beritahu audio controller bahwa state berubah
            audioController?.OnStateChanged(state);
            
            // Reset path saat kembali ke mode Patrol
            if (state == EnemyState.Patrol)
            {
                path = null;      // Hapus jalur yang ada
                pathIndex = 0;    // Reset index jalur
            }
        }

        // Update animator dan arah sprite setiap frame
        UpdateAnimator();
        UpdateFacing();

        // Jalankan behavior sesuai state saat ini
        if (state == EnemyState.Chase)
            KejarPlayer();          // Kejar player
        else if (state == EnemyState.Attack)
            TryAttack();            // Coba serang player
        else if (state == EnemyState.Patrol)
            Patrol();               // Patroli
    }

    // ================= ANIMATOR =================
    // Update parameter animator berdasarkan state dan kondisi musuh
    void UpdateAnimator()
    {
        // Musuh bergerak jika:
        // - Sedang Patrol DAN tidak sedang menunggu di waypoint, ATAU
        // - Sedang Chase
        bool isMoving = (state == EnemyState.Patrol && !isWaitingAtWaypoint) 
                        || state == EnemyState.Chase;
        
        // Set parameter animator untuk animasi berjalan
        enemyAnimator.SetBool("isMoving", isMoving);
        
        // Set parameter animator untuk animasi menyerang
        enemyAnimator.SetBool("isAttacking", state == EnemyState.Attack);
    }

    // ================= FLIP SPRITE =================
    // Update arah hadap sprite musuh
    void UpdateFacing()
    {
        // Prioritas 1: Gunakan arah gerakan terakhir jika ada
        if (lastMoveDirection.x != 0)
        {
            // Flip sprite ke kiri jika bergerak ke kiri (x < 0)
            enemySprite.flipX = lastMoveDirection.x < 0;
        }
        // Prioritas 2: Jika tidak ada gerakan, hadap ke player saat attack
        else if (state == EnemyState.Attack)
        {
            // Flip sprite ke kiri jika player ada di sebelah kiri
            enemySprite.flipX = player.position.x < transform.position.x;
        }
    }

    // ================= PATROL =================
    // Fungsi untuk mengontrol perilaku patroli musuh
    void Patrol()
    {
        // Jika tidak ada patrol point → musuh diam di tempat
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            enemyAnimator.SetBool("isMoving", false);
            return;
        }

        // Jika sedang menunggu di waypoint
        if (isWaitingAtWaypoint)
        {
            // Kurangi timer tunggu
            waypointWaitTimer -= Time.deltaTime;
            
            // Jika waktu tunggu sudah habis
            if (waypointWaitTimer <= 0)
            {
                // Stop menunggu
                isWaitingAtWaypoint = false;
                
                // Pindah ke waypoint berikutnya
                patrolIndex++;
                
                // Jika sudah mencapai waypoint terakhir, kembali ke awal (loop)
                if (patrolIndex >= patrolPoints.Length)
                    patrolIndex = 0;
                
                // Reset path agar dihitung ulang ke waypoint berikutnya
                path = null;
            }
            return; // Keluar dari fungsi karena masih menunggu
        }

        // Dapatkan posisi waypoint target dalam world space
        Vector3 targetWorld = patrolPoints[patrolIndex].position;
        
        // Konversi posisi world ke cell position (grid)
        Vector3Int targetCell = groundTilemap.WorldToCell(targetWorld);
        
        // Konversi posisi musuh ke cell position (grid)
        Vector3Int enemyCell = groundTilemap.WorldToCell(transform.position);

        // Hitung jalur A* jika belum ada atau sudah sampai di akhir jalur
        if (path == null || pathIndex >= path.Count)
        {
            // Minta pathfinding untuk menghitung jalur dari posisi musuh ke target
            path = pathfinding.CariJalur(enemyCell, targetCell);
            
            // Reset index jalur ke awal
            pathIndex = 0;

            // Jika tidak ada jalur yang ditemukan (terhalang/tidak bisa dicapai)
            if (path == null || path.Count == 0)
            {
                // Skip ke waypoint berikutnya
                patrolIndex++;
                
                // Loop kembali ke awal jika sudah di waypoint terakhir
                if (patrolIndex >= patrolPoints.Length)
                    patrolIndex = 0;
                    
                return; // Keluar dari fungsi
            }
        }

        // Gerakkan musuh mengikuti jalur A*
        GerakAStar();

        // Cek apakah musuh sudah sampai di waypoint target
        float distanceToWaypoint = Vector2.Distance(transform.position, targetWorld);
        
        // Jika jarak ke waypoint < 0.2 unit (sudah sampai)
        if (distanceToWaypoint < 0.2f)
        {
            // Mulai tunggu di waypoint
            isWaitingAtWaypoint = true;
            
            // Set timer tunggu
            waypointWaitTimer = waypointWaitTime;
            
            // Reset arah gerakan
            lastMoveDirection = Vector3.zero;
        }
    }

    // ================= CHASE =================
    // Fungsi untuk mengejar player
    void KejarPlayer()
    {
        // Konversi posisi musuh ke cell position
        Vector3Int enemyCell = groundTilemap.WorldToCell(transform.position);
        
        // Konversi posisi player ke cell position
        Vector3Int playerCell = groundTilemap.WorldToCell(player.position);

        // Hitung ulang jalur A* setiap frame
        // (karena player bergerak, jalur perlu di-update terus)
        path = pathfinding.CariJalur(enemyCell, playerCell);
        
        // Reset index ke awal jalur
        pathIndex = 0;

        // Gerakkan musuh mengikuti jalur
        GerakAStar();
    }

    // ================= ATTACK =================
    // Fungsi untuk mencoba menyerang player
    void TryAttack()
    {
        // Cek apakah cooldown masih aktif
        // Jika waktu sekarang < waktu serangan terakhir + cooldown → belum bisa serang
        if (Time.time < lastAttackTime + attackCooldown)
            return; // Keluar dari fungsi

        // Ambil komponen PlayerHealth dari player
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        // Jika player memiliki komponen PlayerHealth
        if (playerHealth != null)
        {
            // Berikan damage ke player
            playerHealth.TakeDamage(attackDamage);
            
            // Catat waktu serangan ini
            lastAttackTime = Time.time;
        }
    }

    // ================= MOVEMENT DENGAN A* =================
    // Fungsi untuk menggerakkan musuh mengikuti jalur A*
    void GerakAStar()
    {
        // Jika tidak ada jalur atau jalur kosong → tidak bergerak
        if (path == null || path.Count == 0) 
        {
            lastMoveDirection = Vector3.zero;
            return;
        }

        // Pastikan index tidak melebihi panjang jalur
        if (pathIndex >= path.Count)
        {
            lastMoveDirection = Vector3.zero;
            return;
        }

        // Konversi cell position target ke world position
        // + setengah cell size agar target di tengah cell
        Vector3 targetWorld = groundTilemap.CellToWorld(path[pathIndex]) 
                            + groundTilemap.cellSize / 2f;

        // Simpan posisi sebelum bergerak (untuk menghitung arah)
        Vector3 oldPos = transform.position;

        // Gerakkan musuh menuju target dengan kecepatan tertentu
        // MoveTowards akan bergerak perlahan menuju target
        transform.position = Vector2.MoveTowards(
            transform.position,     // Posisi saat ini
            targetWorld,            // Posisi target
            speed * Time.deltaTime  // Jarak pergerakan frame ini
        );

        // Hitung arah gerakan (posisi baru - posisi lama)
        // Digunakan untuk flip sprite
        lastMoveDirection = transform.position - oldPos;

        // Jika sudah sangat dekat dengan node target (< 0.05 unit)
        if (Vector2.Distance(transform.position, targetWorld) < 0.05f)
        {
            // Lanjut ke node berikutnya dalam jalur
            pathIndex++;
        }
    }

    // ================= GIZMOS (DEBUGGING) =================
    // Fungsi untuk menggambar visualisasi di Scene view (untuk debugging)
    void OnDrawGizmosSelected()
    {
        // Gambar lingkaran detection radius (warna kuning)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Gambar lingkaran attack range (warna merah)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Gambar jalur patrol (warna cyan)
        if (patrolPoints != null && patrolPoints.Length > 1)
        {
            Gizmos.color = Color.cyan;
            
            // Loop semua patrol points
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    // Gambar sphere di setiap waypoint
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
                    
                    // Hitung index waypoint berikutnya (dengan loop)
                    int nextIndex = (i + 1) % patrolPoints.Length;
                    
                    // Gambar garis ke waypoint berikutnya
                    if (patrolPoints[nextIndex] != null)
                    {
                        Gizmos.DrawLine(
                            patrolPoints[i].position,      // Dari waypoint ini
                            patrolPoints[nextIndex].position // Ke waypoint berikutnya
                        );
                    }
                }
            }
        }

        // Gambar jalur A* yang sedang aktif (warna hijau)
        if (path != null && path.Count > 0 && groundTilemap != null)
        {
            Gizmos.color = Color.green;
            
            // Loop semua node dalam jalur
            for (int i = 0; i < path.Count; i++)
            {
                // Konversi cell position ke world position
                Vector3 cellWorld = groundTilemap.CellToWorld(path[i]) 
                                  + groundTilemap.cellSize / 2f;
                
                // Gambar sphere di setiap node
                Gizmos.DrawSphere(cellWorld, 0.15f);
                
                // Jika bukan node terakhir
                if (i < path.Count - 1)
                {
                    // Hitung posisi node berikutnya
                    Vector3 nextCellWorld = groundTilemap.CellToWorld(path[i + 1]) 
                                          + groundTilemap.cellSize / 2f;
                    
                    // Gambar garis dari node ini ke node berikutnya
                    Gizmos.DrawLine(cellWorld, nextCellWorld);
                }
            }
        }
    }
}