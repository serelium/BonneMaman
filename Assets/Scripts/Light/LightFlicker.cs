using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float _maxIntensity;
    [SerializeField] private float _minIntensity;

    private Light _light;
    private bool _max;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _max = true;
        _light.intensity = _minIntensity;

        StartCoroutine(FlickerLight());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator FlickerLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 6));

            int random = Random.Range(0, 2);
            int count = random == 0 ? 2 : 4;

            if (count == 2)
                count = 2;

            for(int i = 0; i < count; i++)
            {
                //_light.intensity = _max ? _maxIntensity : _minIntensity;

                float duration = 0.05f;
                float startIntensity = _light.intensity;
                float endIntensity = _max ? _maxIntensity : _minIntensity;

                yield return StartCoroutine(LerpLightIntensity(startIntensity, endIntensity, duration));

                _max = !_max;

                yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            }
        }
    }

    private IEnumerator LerpLightIntensity(float start, float end, float duration)
    {
        float time = 0;

        while(time <= duration)
        {
            time += Time.deltaTime;
            _light.intensity = Mathf.Lerp(start, end, time / duration);

            yield return null;
        }
    }
}
