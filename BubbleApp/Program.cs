using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Media;


public class Bubble
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Radius { get; set; }
    public int Speed { get; set; }

    public Bubble(int x, int y, int radius, int speed)
    {
        X = x;
        Y = y;
        Radius = radius;
        Speed = speed;
    }

    public void Move()
    {
        Y -= Speed;

    }
    public bool IsPopped { get; private set; }

    public void Pop()
    {
        IsPopped = true;
    }

}
public class BubbleForm : Form
{
    private List<Bubble> unpoppedBubbles;
    private List<Bubble> poppedBubbles;
    private Random random = new Random();

    private SoundPlayer player;
    private System.Threading.Timer timer;

    public BubbleForm()
    {
        unpoppedBubbles = new List<Bubble>();
        poppedBubbles = new List<Bubble>();



        timer = new System.Threading.Timer(TimerCallback, null, 0, 100);

        player = new SoundPlayer();
        player.SoundLocation = "C:\\Users\\Zeldris\\Downloads\\pop.wav";



        this.DoubleBuffered = true;
        this.BackColor = Color.White;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(800, 600);


    }
    private void TimerCallback(object state)
    {
        // Generates a new bubble at random X position at the Bottom of the page and it should rise up to the top 
        int x = random.Next(this.ClientSize.Width);
        int radius = random.Next(10, 50);
        int speed = random.Next(1, 5);
        Bubble bubble = new Bubble(x, this.ClientSize.Height, radius, speed);
        unpoppedBubbles.Add(bubble);

        foreach (var b in unpoppedBubbles.ToArray())
        {
            b.Move();

            // Remove bubbles when they reach the top
            if (b.Y + b.Radius < 0 || b.Y - b.Radius > this.ClientSize.Height)
            {
                unpoppedBubbles.Remove(b);
            }
        }
        // redraw the bubbles
        this.Invalidate();
    }
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        foreach (var bubble in unpoppedBubbles.ToArray())
        {
            if (!bubble.IsPopped && Distance(e.Location, new Point(bubble.X, bubble.Y)) <= bubble.Radius)
            {
                bubble.Pop();
                player.Play();
                unpoppedBubbles.Remove(bubble);
                poppedBubbles.Add(bubble);
                this.Invalidate();
            }
        }
    }
    private double Distance(Point p1, Point p2)
    {
        int dx = p1.X - p2.X;
        int dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private void Timer_Tick(object sender, EventArgs e)

    {
      


    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        // Draw unpopped bubbles
        foreach (var bubble in unpoppedBubbles)
        {
            e.Graphics.FillEllipse(Brushes.Blue, bubble.X - bubble.Radius, bubble.Y -  bubble.Radius, bubble.Radius * 2, bubble.Radius * 2);
        }

        foreach (var bubble in poppedBubbles)
        {
            e.Graphics.FillEllipse(Brushes.Red, bubble.X - bubble.Radius, bubble.Y - bubble.Radius, bubble.Radius * 2, bubble.Radius * 2);
        }
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);   
        Application.Run(new BubbleForm());
    }
   
       
  }
