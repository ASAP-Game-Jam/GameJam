using UnityEngine;

internal class Battery: MonoBehaviour {
    //Добавляется на обьект, за который будут начисляться очки когда игрок нажимает на него
    [SerializeField] private int score = 5;

    // Создадим на пункт ниже
    private LevelManager levelManager;

    // Функция для установки менеджера
    public void SetLevelManager (LevelManager levelManager) {
        this.levelManager = levelManager;
    }

    
}

/*Для того чтобы Battery работал нужно:

1.Создать обьект
2. Добавить на него компоненты Coins и Collider
3. В колладере поставить флаг IsTrigger*/