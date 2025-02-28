try:
    import tkinter as tk
    from tkinter import ttk
except ImportError:
    print("Error: Tkinter is not installed. Please install it using 'sudo apt-get install python3-tk' on Linux or ensure you have Python's Tkinter module available.")
    exit()

class BlackjackGambit:
    def __init__(self, root):
        self.root = root
        self.root.title("🃏 Blackjack Gambit 🃏")
        self.root.geometry("800x600")
        self.root.configure(bg="#0B3D91")
        self.root.state("zoomed")  # Make fullscreen
        
        self.training_mode = self.training_mode_definition
        self.main_menu()
    
    def main_menu(self):
        """Main menu with Play and Training buttons"""
        self.clear_screen()
        
        frame = tk.Frame(self.root, bg="#0B3D91")
        frame.pack(expand=True)
        
        title = tk.Label(frame, text="🃏 Blackjack Gambit 🃏", font=("Arial", 32, "bold"), fg="white", bg="#0B3D91")
        title.pack(pady=50)
        
        play_button = tk.Button(frame, text="🎮 Play", font=("Arial", 20, "bold"), bg="#FFD700", fg="black", relief="raised", bd=5, command=self.setup_game)
        play_button.pack(pady=20, ipadx=20, ipady=10)
        
        training_button = tk.Button(frame, text="📚 Training", font=("Arial", 20, "bold"), bg="#FFD700", fg="black", relief="raised", bd=5, command=self.training_mode)
        training_button.pack(pady=20, ipadx=20, ipady=10)
    
    def setup_game(self):
        """Game setup screen to select difficulty and number of decks"""
        self.clear_screen()
        
        frame = tk.Frame(self.root, bg="#0B3D91")
        frame.pack(expand=True)
        
        tk.Label(frame, text="🎚️ Select Difficulty", font=("Arial", 20, "bold"), fg="white", bg="#0B3D91").pack(pady=20)
        self.difficulty = ttk.Combobox(frame, values=["Easy", "Normal", "Hard"], font=("Arial", 18))
        self.difficulty.pack(pady=10, ipadx=10, ipady=10)
        self.difficulty.current(0)
        
        tk.Label(frame, text="🃏 Select Number of Decks", font=("Arial", 20, "bold"), fg="white", bg="#0B3D91").pack(pady=20)
        self.deck_count = ttk.Combobox(frame, values=[str(i) for i in range(1, 9)], font=("Arial", 18))
        self.deck_count.pack(pady=10, ipadx=10, ipady=10)
        self.deck_count.current(0)
        
        start_button = tk.Button(frame, text="▶️ Start Game", font=("Arial", 20, "bold"), bg="#32CD32", fg="black", relief="raised", bd=5, command=self.start_game)
        start_button.pack(pady=30, ipadx=20, ipady=10)
        
        back_button = tk.Button(frame, text="⬅️ Back", font=("Arial", 18, "bold"), bg="#FF6347", fg="black", relief="raised", bd=5, command=self.main_menu)
        back_button.pack(pady=10, ipadx=20, ipady=10)
    
    def training_mode_definition(self):
        """Training mode placeholder"""
        self.clear_screen()
        
        frame = tk.Frame(self.root, bg="#0B3D91")
        frame.pack(expand=True)
        
        tk.Label(frame, text="📚 Card Counting Training Mode", font=("Arial", 24, "bold"), fg="white", bg="#0B3D91").pack(pady=50)
        
        back_button = tk.Button(frame, text="⬅️ Back", font=("Arial", 18, "bold"), bg="#FF6347", fg="black", relief="raised", bd=5, command=self.main_menu)
        back_button.pack(pady=20, ipadx=20, ipady=10)
    
    def start_game(self):
        """Placeholder for starting the game"""
        self.clear_screen()
        
        frame = tk.Frame(self.root, bg="#0B3D91")
        frame.pack(expand=True)
        
        difficulty = self.difficulty.get()
        decks = self.deck_count.get()
        
        tk.Label(frame, text=f"🎮 Starting Game\n🛠️ Difficulty: {difficulty}\n🃏 Decks: {decks}", font=("Arial", 22), fg="white", bg="#0B3D91").pack(pady=50)
        
        back_button = tk.Button(frame, text="⬅️ Back", font=("Arial", 18, "bold"), bg="#FF6347", fg="black", relief="raised", bd=5, command=self.setup_game)
        back_button.pack(pady=20, ipadx=20, ipady=10)
    
    def clear_screen(self):
        """Remove all widgets from the screen"""
        for widget in self.root.winfo_children():
            widget.destroy()

if __name__ == "__main__":
    root = tk.Tk()
    app = BlackjackGambit(root)
    root.mainloop()
