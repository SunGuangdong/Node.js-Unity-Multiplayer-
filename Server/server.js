// require()异步加载socket.io,  然后执行回调函数构造一个IO
var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');

// 输出一个Log
console.log('server started');

// 存储所有玩家ID
var players = [];

// 客户端连接的时间响应
io.on('connection',function(socket){
    
    // 生成一个唯一标识
    var thisClientId = shortid.generate();
    // players.push(thisClientId);

    var player = {
        id: thisClientId,
        x:0,
        y:0
    };
    players[thisClientId] = player;

    console.log('客户端被连接, broadcasting spawn , id:', thisClientId);

    // 通知客户端  把 自己添加到 Players 列表中：
    socket.emit('register', { id: thisClientId });

    // 要进行广播，只需添加一个广播的标志将emit ，和调用发送方法。广播是指发送一个消息给每个人（除了启动socket的对象）
    socket.broadcast.emit('spawn', { id: thisClientId });

    // 通知 其他客户端  让他们请求更新位置
    socket.broadcast.emit('requestPosition');

    for (var playerId in players)
    {
        if(playerId == thisClientId)
            continue;

        socket.emit('spawn', players[playerId]);
        console.log('sending spawn to new player for id: ', playerId);
    }
    
    // 处理客户端的 move 请求
    socket.on('move', function(data){
        data.id = thisClientId;
        console.log("Client moved", JSON.stringify(data));

        player.x = data.x;
        player.y = data.y;

        // 通知其他的客户端            
        socket.broadcast.emit('move', data);
    });

    socket.on('updatePosition', function(data) {
       console.log("update position: ", data);
       data.id = thisClientId;

       socket.broadcast.emit('updatePosition', data);
   });

    // 处理 客户端的  跟随 请求
    socket.on('follow', function(data) {
       console.log("follow request: ", data);
       data.id = thisClientId;

        // 让其他客户端 同步处理 这个请求。
       socket.broadcast.emit('follow', data);
   });

    // 处理 客户端的  攻击 请求
    socket.on('attack', function(data) {
       console.log("attack request: ", data);
       data.id = thisClientId;

       io.emit('attack', data);     // 发送给所有客户端
       // socket.broadcast.emit('attack', data);  // 发送给 除了 自己以外的所有客户端
   });
    
    // 客户端 移除的事件响应
    socket.on('disconnect', function(data){
        console.log('客户端被移除');
        
        // 重新集合，移除了 thisClientId 
        // players.splice(players.indexOf(thisClientId), 1);
        delete players[thisClientId];

        socket.broadcast.emit('disconnected', { id: thisClientId });
    });

})

//var players = [];
//
//var playerSpeed = 3;
//
//io.on('connection', function(socket){
//    
//    var thisClientId = shortid.generate();
//    
//    var player = {
//        id: thisClientId,
//        destination: {
//        x: 0, 
//        y: 0
//        },
//        lastPosition: {
//            x: 0,
//            y: 0
//        },
//        lastMoveTime: 0
//    };
//    
//    players[thisClientId] = player;
//    
//    console.log('client connected, broadcasting spawn, id:', thisClientId);
//
//    socket.emit('register', { id: thisClientId });
//	socket.broadcast.emit('spawn', { id: thisClientId });
//    socket.broadcast.emit('requestPosition');
//    
//    for(var playerId in players){
//        
//        if(playerId == thisClientId)
//            continue;
//        
//        socket.emit('spawn', players[playerId]);
//        console.log('sending spawn to new player for id: ', playerId);
//    };
//	
//	socket.on('move', function(data) {
//        data.id = thisClientId;
//		console.log('client moved', JSON.stringify(data));
//        
//        player.destination.x = data.d.x;
//        player.destination.y = data.d.y;
//        
//        console.log("distance between current and destination: ", lineDistance(data.c, data.d));
//        
//        var elapsedTime = Date.now() - player.lastMoveTime;
//        
//        
//        var travelDistanceLimit = elapsedTime * playerSpeed / 1000;
//        
//        var requestedDistanceTraveled = lineDistance(player.lastPosition, data.c);
//        
//        console.log("travelDistanceLimit:", travelDistanceLimit, "requestedDistanceTraveled", requestedDistanceTraveled);
//        
//        if(requestedDistanceTraveled > travelDistanceLimit)
//            //we know they are cheating
//        
//        player.lastMoveTime = Date.now();
//        
//        player.lastPosition = data.c;
//        
//        delete data.c;
//        
//        data.x = data.d.x;
//        data.y = data.d.y;
//        
//        delete data.d;
//        
//        
//		socket.broadcast.emit('move', data);
//	});
//})
//
//function lineDistance(vectorA, vectorB) {
//    var xs = 0;
//    var ys = 0;
//    
//    xs = vectorB.x - vectorA.x;
//    xs = xs * xs;
//    
//    ys = vectorB.y - vectorA.y;
//    ys = ys * ys;
//    
//    return Math.sqrt( xs + ys );
//}