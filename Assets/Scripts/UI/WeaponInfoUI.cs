using Avocado.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*---------------------------------------------------------------------------------------------
WeaponInfoUI es un componente de UI pensado para mostrar los detalles de un arma específica 
(como su ícono, nombre y descripción) cuando, por ejemplo, el jugador la selecciona en un menú 
o la recoge. El método PopulateUI se llama desde otro script y actualiza los elementos visuales 
en pantalla utilizando los datos del WeaponDataSO.
---------------------------------------------------------------------------------------------*/

namespace Avocado.UI
{
    // Este componente se encarga de mostrar la información detallada de un arma en la UI
    public class WeaponInfoUI : MonoBehaviour
    {
        [Header("Dependencies")]
        // Imagen donde se mostrará el icono del arma
        [SerializeField] private Image weaponIcon;

        // Texto donde se mostrará el nombre del arma
        [SerializeField] private TMP_Text weaponName;

        // Texto donde se mostrará la descripción del arma
        [SerializeField] private TMP_Text weaponDescription;

        // Referencia interna a los datos del arma que se están mostrando
        private WeaponDataSO weaponData;

        // Método público para llenar la UI con los datos del arma proporcionada
        public void PopulateUI(WeaponDataSO data)
        {
            // Si no se proporciona un arma válida, salir del método
            if (data is null)
                return;

            // Guardar los datos del arma internamente
            weaponData = data;

            // Asignar los valores de los datos del arma a los elementos de la UI
            weaponIcon.sprite = weaponData.Icon;
            weaponName.SetText(weaponData.Name);
            weaponDescription.SetText(weaponData.Description);
        }
    }
}
