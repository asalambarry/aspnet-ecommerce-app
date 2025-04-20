classDiagram
    class Product {
        +int Id
        +string Name
        +string Description
        +decimal Price
        +string ImageUrl
        +int Stock
        +int CategoryId
    }

    class Category {
        +int Id
        +string Name
        +string Description
    }

    class CartItem {
        +int Id
        +string UserId
        +int ProductId
        +int Quantity
    }

    class Order {
        +int Id
        +string UserId
        +DateTime OrderDate
        +decimal TotalAmount
        +string Status
    }

    class OrderItem {
        +int Id
        +int OrderId
        +int ProductId
        +int Quantity
        +decimal UnitPrice
    }

    Product "1" -- "1" Category : Belongs to
    CartItem "0..*" -- "1" Product : Contains
    Order "1" -- "0..*" OrderItem : Has
    OrderItem "0..*" -- "1" Product : References