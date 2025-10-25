using System;
using System.Linq;
using Week4DoublyLinkedLists.Core;

namespace Week4DoublyLinkedLists.Applications
{
    public class Song
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public TimeSpan Duration { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        
        public Song(string title, string artist, TimeSpan duration, string album = "", int year = 0, string genre = "")
        {
            Title = title;
            Artist = artist;
            Duration = duration;
            Album = album;
            Year = year;
            Genre = genre;
        }
        
        public override string ToString()
        {
            return $"{Title} by {Artist} ({Duration:mm\\:ss})";
        }
        
        public string ToDetailedString()
        {
            return $"{Title} - {Artist} [{Album}, {Year}] ({Duration:mm\\:ss}) [{Genre}]";
        }
    }

    public class MusicPlaylist
    {
        #region Step 9: Playlist Core Structure
        
        private DoublyLinkedList<Song> playlist;
        private Node<Song>? currentSong;
        
        public string Name { get; set; }
        public int TotalSongs => playlist.Count;
        public bool HasSongs => playlist.Count > 0;
        public Song? CurrentSong => currentSong?.Data;
        
        public MusicPlaylist(string name = "My Playlist")
        {
            Name = name;
            playlist = new DoublyLinkedList<Song>();
            currentSong = null;
        }
        
        #endregion
        
        #region Step 10a: Adding Songs
        
        public void AddSong(Song song)
        {
            if (song == null)
                throw new ArgumentNullException(nameof(song), "Song cannot be null.");

            playlist.AddLast(song);

            if (currentSong == null)
                currentSong = playlist.Last;
        }

        public void AddSongAt(int position, Song song)
        {
            if (position < 0 || position > playlist.Count)
                throw new ArgumentOutOfRangeException(nameof(position), "Invalid position.");

            if (song == null)
                throw new ArgumentNullException(nameof(song), "Song cannot be null.");

            playlist.Insert(position, song);

            if (playlist.Count == 1)
                currentSong = playlist.First;
        }

        #endregion
        
        #region Step 10b: Removing Songs
        
        public bool RemoveSong(Song song)
        {
            if (song == null)
                throw new ArgumentNullException(nameof(song), "Song cannot be null.");

            var node = playlist.Find(song);
            if (node == null)
                return false;

            if (currentSong != null && EqualityComparer<Song>.Default.Equals(currentSong.Data, song))
            {
                if (node.Next != null)
                    currentSong = node.Next;
                else if (node.Previous != null)
                    currentSong = node.Previous;
                else
                    currentSong = null;
            }

            return playlist.Remove(song);
        }

        public bool RemoveSongAt(int position)
        {
            if (position < 0 || position >= playlist.Count)
                throw new ArgumentOutOfRangeException(nameof(position), "Position is out of range.");

            // Changed: make GetNodeAt public/internal in DoublyLinkedList.cs
            var node = playlist.GetNodeAt(position);
            if (node == null)
                return false;

            if (currentSong != null && EqualityComparer<Song>.Default.Equals(currentSong.Data, node.Data))
            {
                if (node.Next != null)
                    currentSong = node.Next;
                else if (node.Previous != null)
                    currentSong = node.Previous;
                else
                    currentSong = null;
            }

            playlist.RemoveAt(position);
            return true;
        }

        #endregion
        
        #region Step 10c: Navigation
        
        public bool Next()
        {
            if (currentSong == null || currentSong.Next == null)
                return false;

            currentSong = currentSong.Next;
            return true;
        }

        public bool Previous()
        {
            if (currentSong == null || currentSong.Previous == null)
                return false;

            currentSong = currentSong.Previous;
            return true;
        }

        public bool JumpToSong(int position)
        {
            if (position < 0 || position >= playlist.Count)
                return false;

            // Changed: make GetNodeAt public/internal in DoublyLinkedList.cs
            var node = playlist.GetNodeAt(position);
            if (node == null)
                return false;

            currentSong = node;
            return true;
        }

        #endregion
        
        #region Step 11: Display and Basic Management
        
        public void DisplayPlaylist()
        {
            Console.WriteLine($"\n🎵 Playlist: {Name} ({playlist.Count} songs)");

            if (!playlist.Any())
            {
                Console.WriteLine("   [No songs in the playlist]");
                return;
            }

            int index = 1;
            foreach (var song in playlist)
            {
                string marker = (currentSong != null && EqualityComparer<Song>.Default.Equals(song, currentSong.Data))
                                ? "► "
                                : "   ";

                Console.WriteLine($"{marker}{index,2}. {song.Title} by {song.Artist} ({song.Duration:mm\\:ss})");
                index++;
            }
        }

        public void DisplayCurrentSong()
        {
            if (currentSong == null || currentSong.Data == null)
            {
                Console.WriteLine("🎵 No song is currently selected in the playlist.");
                return;
            }

            int position = GetCurrentPosition();
            int total = TotalSongs;
            var song = currentSong.Data;

            Console.WriteLine("=== Now Playing ===");
            Console.WriteLine($"► {song.Title} by {song.Artist}");
            Console.WriteLine($"Album: {song.Album}");
            Console.WriteLine($"Year: {song.Year}");
            Console.WriteLine($"Duration: {song.Duration:mm\\:ss}");
            Console.WriteLine($"Genre: {song.Genre}");
            Console.WriteLine($"Position: Song {position} of {total}");
        }

        public Song? GetCurrentSong()
        {
            return currentSong?.Data;
        }

        #endregion
        
        #region Helper Methods
        
        public int GetCurrentPosition()
        {
            if (currentSong == null) return 0;
            
            int position = 1;
            var current = playlist.First;
            while (current != null && current != currentSong)
            {
                position++;
                current = current.Next;
            }
            return current == currentSong ? position : 0;
        }

        public TimeSpan GetTotalDuration()
        {
            TimeSpan total = TimeSpan.Zero;
            foreach (var song in playlist)
            {
                total = total.Add(song.Duration);
            }
            return total;
        }

        #endregion
    }
}
