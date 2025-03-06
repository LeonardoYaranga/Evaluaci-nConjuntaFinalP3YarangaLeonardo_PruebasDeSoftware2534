//using System;
//using Reqnroll;
//using TDDTestingMVC.Models;

//namespace ReqnrollTestProject.StepDefinitions
//{
//    [Binding]
//    public class CirculoStepDefinitions
//    {
//        // For additional details on Reqnroll step definitions see https://go.reqnroll.net/doc-stepdef
//        private readonly Circle _circle = new Circle();
//        private float _resultado;

//        [Given("the radius of the circle is {float}")]
//        public void GivenTheRadiusOfTheCircleIs(Decimal p0)
//        {

//            _circle.radius = (float)p0;

//               }

//        [When("the area of the circle is calculated")]
//        public void WhenTheAreaOfTheCircleIsCalculated()
//        {
//            _resultado = (float)_circle.CalculateArea();
//                }

//        [Then("the result should be {float}")]
//        public void ThenTheResultShouldBe(Decimal p0)
//        {
//            _resultado.CompareTo((float)p0);
//        }
//    }
//}
