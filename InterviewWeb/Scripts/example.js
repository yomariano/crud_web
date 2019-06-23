var root = root || {};

// What is this construct known as?
// Immediately Invoked Function Expression
// What purpose does it serve and what problem could it help prevent?
// It helps to prevent for global variables to be overlap. It limits the scope.
// Which functions could external script call and how?
// sayHello and sayGoodbye
// root.demo.sayHello() and root.demo.sayGoodbye()
(function () {
    function sayHello() {
        console.log("Hello");
    }

    function sayGoodbye() {
        console.log("Goodbye");
    }

    function shoutHello() {
        console.log("HELLO");
    }

    root.demo = {
        sayHello: sayHello,

        sayGoodbye: sayGoodbye
    };
}());