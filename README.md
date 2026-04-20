# 🎮 Pokemon API

REST API untuk manajemen data Pokémon yang dibangun menggunakan ASP.NET Core Web API dan PostgreSQL. API ini mencakup operasi CRUD lengkap untuk data pokémon, tipe pokémon, dan gerakan (move) milik masing-masing pokémon.

---

## 📌 Deskripsi Project

Domain yang dipilih adalah **manajemen data Pokémon**. Sistem ini memungkinkan pengguna untuk mengelola data pokémon beserta tipe dan gerakan serangannya melalui REST API.

Relasi antar data:
- Setiap **pokémon** memiliki satu **tipe** (misalnya Electric, Fire, Water)
- Setiap **pokémon** dapat memiliki banyak **gerakan/move**

Project ini dibuat sebagai tugas LKM 01 mata kuliah Pemrograman Antarmuka Aplikasi, Universitas Jember.

---

## 🛠️ Teknologi

| Komponen        | Detail                        |
|-----------------|-------------------------------|
| Bahasa          | C#                            |
| Framework       | ASP.NET Core Web API (.NET 8) |
| Database        | PostgreSQL                    |
| Library DB      | Npgsql (ADO.NET for PostgreSQL)|
| Tools Pengujian | Postman                       |
| IDE             | Visual Studio 2022            |

---

## 📁 Struktur Folder

```
PokemonApi/
├── Controllers/
│   └── PokemonController.cs   # PokemonController, TipeController, MoveController
├── Helpers/
│   ├── ApiResponse.cs         # Helper untuk format response JSON yang konsisten
│   └── SqlDBHelper.cs         # Helper untuk manajemen koneksi Npgsql
├── Models/
│   ├── Models.cs              # Definisi class model dan request DTO
│   └── PokemonContext.cs      # Semua logika query CRUD ke database
├── appsettings.json           # Konfigurasi koneksi database
├── database.sql               # DDL tabel dan sample data
└── README.md
```

---

## ⚙️ Instalasi

### Prasyarat
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) + pgAdmin 4
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Postman](https://www.postman.com/downloads/)

### Langkah Instalasi

1. **Clone repository ini**
   ```bash
   git clone <url-repository>
   cd PokemonApi
   ```

2. **Buka project di Visual Studio 2022**
   - Double-click file `.sln` atau buka via File → Open → Project/Solution

3. **Install library Npgsql via NuGet**
   - Klik kanan pada nama project di Solution Explorer
   - Pilih **Manage NuGet Packages**
   - Klik tab **Browse**, cari `Npgsql`
   - Klik **Install**

4. **Sesuaikan konfigurasi koneksi database**
   - Buka file `appsettings.json`
   - Ubah nilai `Password` sesuai password PostgreSQL kamu:
   ```json
   "WebApiDatabase": "Host=127.0.0.1; Port=5432; Database=postgres; Username=postgres; Password=YourPasswordHere"
   ```

---

## 🗄️ Import Database

1. Buka **pgAdmin 4**
2. Klik kanan pada database `postgres` → pilih **Query Tool**
3. Klik ikon folder (Open File), lalu pilih file `database.sql`
4. Tekan **F5** atau klik tombol **Execute** untuk menjalankan
5. Pastikan tidak ada error — schema `pokemon_db` dan 3 tabel akan terbuat otomatis beserta sample data

---

## ▶️ Menjalankan Project

Tekan **F5** di Visual Studio 2022, atau jalankan perintah berikut di terminal:

```bash
dotnet run
```

API akan berjalan di `https://localhost:<PORT>/` — port bisa dilihat di console output setelah project dijalankan.

---

## 📋 Daftar Endpoint

### 🔹 Pokemon

| Method   | URL                  | Keterangan                                         |
|----------|----------------------|----------------------------------------------------|
| `GET`    | `/api/pokemon`       | Ambil semua data pokémon (include nama tipe)       |
| `GET`    | `/api/pokemon/{id}`  | Ambil detail satu pokémon berdasarkan ID           |
| `POST`   | `/api/pokemon`       | Tambah pokémon baru                                |
| `PUT`    | `/api/pokemon/{id}`  | Update data pokémon berdasarkan ID                 |
| `DELETE` | `/api/pokemon/{id}`  | Hapus pokémon (move-nya ikut terhapus otomatis)    |

### 🔹 Tipe Pokemon

| Method | URL         | Keterangan                    |
|--------|-------------|-------------------------------|
| `GET`  | `/api/tipe` | Ambil semua tipe pokémon      |

### 🔹 Move (Gerakan)

| Method   | URL                                   | Keterangan                              |
|----------|---------------------------------------|-----------------------------------------|
| `GET`    | `/api/pokemon/{idPokemon}/moves`      | Ambil semua gerakan milik satu pokémon  |
| `POST`   | `/api/pokemon/{idPokemon}/moves`      | Tambah gerakan baru ke pokémon          |
| `DELETE` | `/api/pokemon/{idPokemon}/moves/{id}` | Hapus gerakan berdasarkan ID            |

---

## 📨 Contoh Request & Response

### POST `/api/pokemon` — Tambah pokémon baru

**Request Body:**
```json
{
  "nama": "Eevee",
  "id_tipe": 1,
  "level": 5,
  "hp": 55
}
```

**Response `201 Created`:**
```json
{
  "status": "success",
  "data": {
    "message": "Pokemon berhasil ditambahkan"
  }
}
```

---

### GET `/api/pokemon/1` — Ambil detail pokémon

**Response `200 OK`:**
```json
{
  "status": "success",
  "data": {
    "id_pokemon": 1,
    "nama": "Pikachu",
    "id_tipe": 1,
    "nama_tipe": "Electric",
    "level": 5,
    "hp": 45,
    "created_at": "2026-04-20T10:00:00",
    "updated_at": "2026-04-20T10:00:00"
  }
}
```

---

### GET `/api/pokemon/999` — ID tidak ditemukan

**Response `404 Not Found`:**
```json
{
  "status": "error",
  "message": "Pokemon tidak ditemukan"
}
```

---

### POST `/api/pokemon` — Validasi gagal (nama kosong)

**Response `400 Bad Request`:**
```json
{
  "status": "error",
  "message": "Field 'nama' tidak boleh kosong"
}
```

---

## 🎥 Video Presentasi
(

> Link YouTube: *(isi setelah upload)*
