package.path = "./lua/?.lua;" ..  package.path
local lazynet = require "lazynet"


lazynet.start(function ( ... )
    local serviceID = lazynet.getID();
    lazynet.error(serviceID, "current serviceID=" .. serviceID);
    local bootstrapServiceID = lazynet.getServiceID("bootstrap")
    lazynet.error(bootstrapServiceID, "bootstrap service ID=" .. bootstrapServiceID);
    lazynet.sendMessage(bootstrapServiceID, "say", bootstrapServiceID, "���")

    lazynet.addTrigger("active", function ( ip )
        lazynet.error(serviceID,  ip)
    end)

    lazynet.createSocket();
    lazynet.bind();

end) 