namespace LearningWPF.Models
{
    public class Fighter
    {
        public float Health { get; protected set; }
        public float Damage { get; protected set; }
        public float Armor { get; protected set; }

        public Fighter( float health, float armor, float damage)
        {
            Health = health;
            Armor = armor;
            Damage = damage;
        }

        public string GetFighterStats()
        {
            return $"У противника оказалось {Health} здоровья. Наносит он {Damage} урона. " +
                   $"А защищён он {Armor} единицами брони";
        }

        public void TakeDamage(float damage)
        {
            var trueDamage = damage - (damage * (Armor / 10));
            Health -= trueDamage;
        }

        public void ChangeHealthFor(float value) => Health += value;
        public void ChangeArmorFor(float value) => Armor += value;
        public void ChangeDamageFor(float value) => Damage += value;
    }
}
