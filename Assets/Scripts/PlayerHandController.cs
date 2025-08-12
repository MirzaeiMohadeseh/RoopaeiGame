using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerHandController : MonoBehaviour
{
    void Update()
    {
        if (!ScoreCounter.Instance._isGameActive) 
            return;

        // بررسی آیا کلیک روی UI بوده یا نه
        if (IsPointerOverUIElement())
            return;

        if (Input.GetMouseButtonDown(0) || 
           (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ScoreCounter.Instance.ball.ApplyForce(touchPos);
            ScoreCounter.Instance.RegisterTouch();
        }
    }

    // بررسی آیا اشاره‌گر روی عنصر UI است
    private bool IsPointerOverUIElement()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;
        
        // برای تاچ‌های موبایل
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return true;
        }
        
        return false;
    }
}