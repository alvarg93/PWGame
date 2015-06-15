using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game4
{
    public class HealthBar
    {
        Animation animation;
        double maxHealth;
        double curHealth;
        Color color;
        string bar = "||||||||||";

        public HealthBar(ContentManager content, double maxHealth, Vector2 position)
        {
            animation = new Animation();
            this.maxHealth = maxHealth;
            animation.LoadContent(content, null, bar, position);
        }

        public void SetHealth(double curHealth)
        {
            this.curHealth = curHealth;
            color = Color.Green;
            if (curHealth / maxHealth < 0.7) color = Color.Yellow;
            if (curHealth / maxHealth < 0.3) color = Color.Red;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            animation.Text = bar.Substring(0, Math.Min(Math.Max((int)(curHealth / maxHealth * bar.Length),0),10));
            animation.Color = color;
            animation.Position = position;
            animation.Draw(spriteBatch);

        }
    }
}
